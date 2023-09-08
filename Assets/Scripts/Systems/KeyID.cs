// CREDITS:
// Jose Lopez
// MODIFIED:
//Beyioku Daniel

using UnityEngine;
using TMPro;

public class KeyID : Pickup
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the keyID that equals a doorID")]
    public int keyID;
    [Tooltip("Please assign the prompt text object here")]
    [SerializeField] TextMeshProUGUI PromptText;
    #endregion

    #region OTHER VARIABLES
    private bool playerNear;
    private bool getKey;
    [HideInInspector]
    public bool keyPicked;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;
    }

    // Update is called once per frame
    private void Update()
    {
        ShowPrompt();
        MakePromptDisappear();
        GetKey();
    }
    #endregion

    #region PROMPT METHODS
    // Method to show the UI prompt
    void ShowPrompt()
    {
        if (!playerNear) return;

        if (PromptText.color.a >= 1f) return;

        float alpha = Mathf.Min(1f, PromptText.color.a + Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }

    void MakePromptDisappear()
    {
        if (playerNear) return;

        if (PromptText.color.a <= 0f) return;

        float alpha = Mathf.Max(0f, PromptText.color.a - Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }
    #endregion

    #region GET KEY METHOD
    // Method to Get Key
    void GetKey()
    {
        if (getKey && PlayerInput.Maps.Player.Interact.triggered)
        {
            playerNear = false;
            keyPicked = true;
        }
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = true;
            getKey = true;
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = false;
            getKey = false;
        }
    }
    #endregion
}
