using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenDoorSpaceGame : MonoBehaviour
{
    [Tooltip("Please assign the rawimage")]
    [SerializeField] GameObject spaceGamePanel;

    [Tooltip("Please assign the videoplayer")]
    [SerializeField] GameObject videoplayer;

    private bool IsAtDoor = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsAtDoor == true)
        {
            videoplayer.SetActive(true);
            spaceGamePanel.SetActive(true);
        }
        else
        {
            videoplayer.SetActive(false);
            spaceGamePanel.SetActive(false);
        }
    }

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            IsAtDoor = true;
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            IsAtDoor = false;
        }
    }
    #endregion
}
