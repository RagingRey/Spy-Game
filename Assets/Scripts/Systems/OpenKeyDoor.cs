// CREDITS:
// Jose Lopez
//MODIFIED:
//Beyioku Daniel

using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Inventory;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class OpenKeyDoor : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the doorID that equals a keyID")]
    [SerializeField] int DoorID;
    [Tooltip("Please assign the prompt text object here")]
    [SerializeField] TextMeshProUGUI PromptText;
    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] Animator[] anim;
    #endregion

    #region OTHER VARIABLES
    private bool playerNear;
    private bool doorOpened;
    private InventoryManager inventory;
    private List<KeyID> Keys = new List<KeyID>();
    Collider collider;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    public void Update()
    {
        ShowPrompt();
        MakePromptDisappear();
        OpenDoor();
    }
    #endregion

    #region PROMPT METHODS
    // Method to show the UI prompt
    void ShowPrompt()
    {
        if (!playerNear || doorOpened) return;

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

    #region OPEN DOOR METHODS
    // Method to detect key to open door
    void OpenDoor()
    {
        foreach (KeyID key in Keys)
        { 
            if (key.keyID == DoorID && PlayerInput.Maps.Player.Interact.triggered) 
            { 
                inventory.RemoveItem(key.Id); 
                playerNear = false;
                doorOpened = true;
                OpenDoorAnimation();
            }
        }
    }

    // Method to open doors animation
    void OpenDoorAnimation()
    {
        anim[0].SetBool("MoveLeft", true);
        anim[1].SetBool("MoveRight", true);
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = true;
            inventory = col.gameObject.GetComponent<InventoryManager>();

            if (inventory != null)
            {
                foreach (var item in inventory.InventoryItems.OfType<KeyID>())
                {
                    KeyID tempKey = item;
                    Debug.Log("Found"); 
                    Keys.Add(tempKey);
                }
            }
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            playerNear = false;
            inventory = null;
        }
    }
    #endregion
}
