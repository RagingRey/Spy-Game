// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class DoorMinigame : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the CanvasMinigame")]
    [SerializeField] GameObject CanvasMinigame;
    [Tooltip("Please assign the prompt text object here")]
    [SerializeField] TextMeshProUGUI PromptText;
    [Tooltip("Please assign the CanvasArrow")]
    [SerializeField] Slider arrow;
    
    [Tooltip("Please assign the ArrowSpeed movement")]
    [SerializeField] float arrowSpeed;

    [Tooltip("Please assign the animations of the door to open")]
    [SerializeField] Animator[] anim;
    #endregion

    #region OTHER VARIABLES
    GameObject Zone;
    private bool playerNear;
    private GameObject Hacker;
    private float speed;
    private float pos = 0;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    private void Start()
    {
        playerNear = false;
        Zone = this.gameObject;
    }

    // Update is called once per frame
    private void Update()
    {
        ShowPrompt();
        MakePromptDisappear();
        Minigame();
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

    #region OPEN DOOR METHODS
    // Method when arrow stops at the right zone open door
    public void Minigame()
    {
        speed = arrowSpeed;

        Hacker = GameObject.Find("Hacker(Clone)");
        if (CanvasMinigame.activeSelf)
        {
            pos += speed * Time.deltaTime;
            arrow.value = Mathf.PingPong(pos, arrow.maxValue);
        }
        

        if (Hacker != null && PlayerInput.Maps.Player.Interact.triggered)
        {
            speed = 0;
            if (arrow.value >= 0.350 && arrow.value <=0.650)
            {
                CanvasMinigame.SetActive(false);
                arrow.value = 0;
                OpenDoor();
                Destroy(Zone);
            }
            else
            {
                speed = arrowSpeed;
            }
        }
    }

    // Method to open doors animation
    void OpenDoor()
    {
        anim[0].SetBool("MoveLeft", true);
        anim[1].SetBool("MoveRight", true);
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Hacker(Clone)" && this.enabled == true)
        {
            CanvasMinigame.SetActive(true);
            playerNear = true;
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Hacker(Clone)")
        {
            CanvasMinigame.SetActive(false);
            playerNear = false;
        }
    }
    #endregion
}
