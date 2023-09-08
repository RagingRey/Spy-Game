// CREDITS:
// Joshua Standridge
// Medyan Mehiddine
//Beyioku Daniel
using System.Collections;
using UnityEngine;

public class CharacterSwitch : MonoBehaviour{
    #region INSPECTOR VARIABLES
    [Tooltip("The index of the character (same index in the characters array) to start the game with")]
    [Range(0, 2)]
    [SerializeField] int characterToStartWith;
    [Tooltip("Where the player will be spawned when the game starts")]
    [SerializeField] Transform firstSpawnPosition;
    [Space]
    [SerializeField] GameObject[] characters;
    
    [Space]
    [Header("DEBUGGING")]
    [SerializeField] bool debugging;
    [Tooltip("Writes in the console which character is currently being used")]
    [SerializeField] bool debugCurrentCharacter;
    #endregion

    #region PUBLIC OR HIDDEN VARIABLES
    [HideInInspector] public static GameManager managerInstance;
    #endregion

    #region PRIVATE VARIABLES
    private static int activeCharacter;
    private static bool inSwitchingArea;
    private static Vector3 switchingPosition;

    private static GameObject currentCharacterObject = null;
    ArrayList loadedCharacters = new ArrayList();
    #endregion

    #region GETTERS AND SETTERS
    public static int ActiveCharacter{
        get{
            return activeCharacter;
        }
    }

    public static GameObject ActiveCharacterObject{
        get{
            return currentCharacterObject;
        }
    }

    public Transform GetFirstSpawnPoint()
    {
        return firstSpawnPosition;
    }

    public static bool InsideSwitchArea{ get{return inSwitchingArea;} }

    public static int Scout{ get{return 0;} }
    public static int Hacker{ get{return 1;} }
    public static int Gobro{ get{return 2;} }
    #endregion

    #region EXECUTION
    void Start(){
        bool charactersInOrder = CheckForCharactersOrder();
        if(!charactersInOrder)
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>CHARACTERS NOT IN ORDER</color>"+
                "\nCheck the docs for possible solutions");

        LoadAllCharacters();
        SwitchToChracter(characterToStartWith, firstSpawnPosition.position);
    }

    void Update(){
        if(InsideSwitchArea && PlayerInput.Maps.Player.ChangeCharacter.triggered)
            SwitchToNextCharacter();

        // DEBUGGING
        if(!debugging)
            return;
        DebugActiveCharacter();
    }
    #endregion

    #region CHARACTER SWITCHING
    void SwitchToChracter(int characterIndex, Vector3 switchPosition){
        if(characterIndex < 0 || characterIndex >= characters.Length){
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>FAILED TO SWITCH TO SPECIFIC CHARACTER</color>"+
                "\n Check if the character to start with index is inside of the characters array size");
        }

        if(currentCharacterObject != null)
            currentCharacterObject.SetActive(false);

        activeCharacter = characterIndex;
        currentCharacterObject = (GameObject)loadedCharacters[characterIndex];
        currentCharacterObject.transform.position = switchPosition;

        currentCharacterObject.SetActive(true);
    }

    public void SwitchToNextCharacter(){
        if(!currentCharacterObject.GetComponent<PlayerMovement>().Grounded){
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>FAILED TO SWITCH TO NEXT CHARACTER</color>"+
                        "\nPlayer is not grounded. Possible solution: ground object layer is not set");
        }

        if(currentCharacterObject != null)
            currentCharacterObject.SetActive(false);

        // Get the next character in the array and loop back to 0 if reached the end of the array
        activeCharacter = (activeCharacter + 1) % characters.Length;
        currentCharacterObject = (GameObject)loadedCharacters[activeCharacter];
        currentCharacterObject.transform.position = switchingPosition;

        currentCharacterObject.SetActive(true);
    }
    #endregion

    #region LOADING CHARACTERS
    bool CheckForCharactersOrder(){
        if(characters.Length != 3){
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>FAILED TO LOAD CHARACTERS.</color>"+
                "\nCharacters count is not 3");
        }
        
        if(characters[0].name != "Scout")
            return false;
        if(characters[1].name != "Hacker")
            return false;
        if(characters[2].name != "GoBro")
            return false;

        return true;
    }

    /// <summary>
    /// Loads all characters in the scene while having them disabled,
    /// except for the first one
    /// </summary>
    void LoadAllCharacters(){

        if (characters.Length == 0 || firstSpawnPosition == null){
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>FAILED TO LOAD CHARACTERS</color>"+
                        "\nCheck if first spawn point is assigned or there are any characters in the array");
        }

        for(int i=0; i < characters.Length; i++){
            loadedCharacters.Add(SpawnCharacterInFirstPosition(i));
            ((GameObject)loadedCharacters[i]).SetActive(false);
        }
    }

    /// <summary>
    /// Spawns a character at the level starting spawning position
    /// </summary>
    GameObject SpawnCharacterInFirstPosition(int character){
        if(firstSpawnPosition == null){
            throw new System.Exception($"<color=yellow> Character Switch:</color> <color=red>NO FIRST SPAWN POINT FOUND / ASSIGNED</color>");
        }

        return Instantiate(characters[character], firstSpawnPosition.position, Quaternion.identity);
    }
    #endregion

    #region SWITCHING AREA HANDLING
    public static void SetSwitchArea(Vector3 areaPosition){
        switchingPosition = areaPosition;
        inSwitchingArea = true;
    }

    public static void LeaveSwitchingArea(){
        inSwitchingArea = false;
    }
    #endregion    

    #region DEBUGGING
    void DebugActiveCharacter(){
        if(!debugCurrentCharacter || !Logger.GlobalDebugging)
            return;

        if(activeCharacter == Scout)
            Debug.Log($"Active character: <color=red>Scount</color>");
        else if(activeCharacter == Hacker)
            Debug.Log($"Active character: <color=green>Go Bro</color>");
        else
            Debug.Log($"Active character: <color=blue>Hacker</color>");
    }
    #endregion
}