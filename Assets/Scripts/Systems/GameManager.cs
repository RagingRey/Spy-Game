// CREDITS:
// Joshua Standridge

using UnityEngine;

public class GameManager : MonoBehaviour{

    #region PUBLIC OR HIDDEN VARIABLES
    [HideInInspector] public static GameManager managerInstance;
    #endregion

    #region EXECUTION
    void Awake(){
        // persist between scenes
        DontDestroyOnLoad(this.gameObject);

        // No duplicate managers
        if (managerInstance == null) {
            managerInstance = this;
        } 
        else {
            UnityEngine.Object.Destroy(gameObject);
        }

    }
    #endregion
}
