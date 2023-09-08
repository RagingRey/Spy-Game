using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Hideable : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the sphere collider of the hiding object here")]
    [SerializeField] SphereCollider SphereCollider;
    [Tooltip("Please assign the prompt text object here")]
    [SerializeField] TextMeshProUGUI PromptText;
    [Tooltip("Please assign the hiding spot 1 object here")]
    [SerializeField] Transform hidingSpot1;
    [Tooltip("Please assign the hiding spot 2 object here")]
    [SerializeField] Transform hidingSpot2;

    #endregion

    #region OTHER VARIABLES
    private bool playerNear;
    private GameObject player;
    private bool isHiding;
    private PlayerMovement playerMovementScript;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        playerNear = false;
        isHiding = false;
    }

    // Update is called once per frame
    void Update()
    {
        ShowPrompt();
        MakePromptDisappear();
    }

    #region PROMPT METHODS
    // Method to show the UI prompt
    void ShowPrompt()
    {
        if (isHiding) return;

        if (!playerNear) return;

        if (PromptText.color.a >= 1f) return;

        float alpha = Mathf.Min(1f, PromptText.color.a + Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }

    void MakePromptDisappear()
    {
        if (!isHiding && playerNear) return;

        if (isHiding && !playerNear) return;

        if (PromptText.color.a <= 0f) return;

        float alpha = Mathf.Max(0f, PromptText.color.a - Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            player = other.gameObject;
            playerNear = true;
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerNear = false;
        }
    }
    #endregion

    #region HIDE METHODS
    // Method to initiate when the player press E to hide
    public void Hide()
    {
        TurnOffMovementScript();
        PromptText.text = "Press E to unhide";
        MoveToHidingSpot();

    }

    void TurnOffMovementScript()
    {
        if (player.GetComponent<PlayerMovement>() != null)
        {
            playerMovementScript = player.GetComponent<PlayerMovement>();
            playerMovementScript.isHiding = true;
          //  playerMovementScript.enabled = false;
        }
    }

    void MoveToHidingSpot()
    {
        Vector3 dir = Vector3.zero;
        Vector3 destination = Vector3.zero;
        
        if (Spot1IsCloser())
        {
            dir = (hidingSpot1.position - player.transform.position).normalized;
            destination = hidingSpot1.position;
        }
        else
        {
            dir = (hidingSpot2.position - player.transform.position).normalized;
            destination = hidingSpot2.position;
        }
        
        while (Vector3.Distance(destination, player.transform.position) > 0.01f)
        {
            player.transform.position += dir * Time.deltaTime;
        }
    }

    private bool Spot1IsCloser()
    {
        float distanceToSpot1 = Vector3.Distance(player.transform.position, hidingSpot1.position);
        float distanceToSpot2 = Vector3.Distance(player.transform.position, hidingSpot2.position);
        if (distanceToSpot1 < distanceToSpot2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    #endregion

    #region UNHIDE METHODS
    // Method to initiate when the player press E again to unhide
    public void Unhide()
    {
        if (playerMovementScript != null)
        {
          playerMovementScript.isHiding = false;
            playerMovementScript.enabled = true;
          
        }
        PromptText.text = "Press E to hide";

    }
    #endregion
}
