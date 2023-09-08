using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OpernDoorKeypad : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] TextMeshProUGUI codeText;
    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] GameObject codePanel;

    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] string safeCode;
    
    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] Animator[] anim;

    #endregion

    #region OTHER VARIABLES
    private bool IsAtDoor = false;
    string codeTextValue = "";
    Collider collider;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider>();   
    }

    // Update is called once per frame
    void Update()
    {
        OpenDoor();
    }
    #endregion

    #region OPEN DOOR METHODS

    void OpenDoor()
    {
        codeText.text = codeTextValue;

        if(codeTextValue == safeCode)
        {
            OpenDoorAnim();
            codePanel.SetActive(false);
            Destroy(collider);
        }
        
        if(codeTextValue.Length >= 4)
        {
            codeTextValue = "";
        }
    }

    //
    public void AddDigit(string digit)
    {
        codeTextValue += digit;
    }

    // Method to open doors animation
    void OpenDoorAnim()
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
            IsAtDoor = true;
            codePanel.SetActive(true);
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            IsAtDoor = false;
            codePanel.SetActive(false);
        }
    }
    #endregion
}
