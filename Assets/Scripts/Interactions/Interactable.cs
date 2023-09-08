// Credits:
// Medyan Mehiddine
// Beyioku Daniel

using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class Interactable : MonoBehaviour{

    #region INSPECTOR VARIABLES
    [Tooltip("Unchecked: Player can interact once with the object" +
             "Checked: Player can interact multiple times with the object"+
             "(Does not update during runtime)")]
    [SerializeField] bool multipleInteractions;
    [SerializeField] bool holdButtonToInteraction;
    [Range(-1f, 120f)] [Tooltip("The delay (in seconds) before letting the player interact again. (Negative delay means no delay)")]
    [SerializeField] float timeBeforeNextInteraction = 1f;

    [Space]
    [Tooltip("The function to be executed when the player interacts with this object")]
    [SerializeField] UnityEvent OnInteract;
    [Tooltip("The function to be executed when the player leaves an interacted object")]
    [SerializeField] UnityEvent OnLeaveInteract;
    [Tooltip("If the interactable object is an object for hiding")]
    [SerializeField] bool ObjectForHiding;

    [Space]
    [Header("REQUIRED ELEMENTS")]
    [SerializeField] protected string playerTag;
    [SerializeField] SphereCollider sphereCollider;
    [SerializeField] GameObject interactionPrompt;
    #endregion

    #region PRIVATE AND HIDDEN VARIABLES
    protected bool canInteract = true;
    protected bool interactInputHeld;
    protected bool isHiding;

    // CACHE
    bool interactionDelayCache;
    #endregion

    void Awake(){
        if(sphereCollider != null && !sphereCollider.isTrigger)
            sphereCollider.isTrigger = true;
    }

    void Start() {
        PlayerInput.Maps.Player.Interact.performed += _ => interactInputHeld = true;
        PlayerInput.Maps.Player.Interact.canceled += _ => interactInputHeld = false;

        isHiding = false;
    }

    public void OnTriggerStay(Collider other) {
        if(!other.CompareTag(playerTag))
            return;
        
        DisplayInteractionUI();

        if (!ObjectForHiding)
        {
            if (holdButtonToInteraction)
            {
                if (interactInputHeld)
                {
                    if (!canInteract)
                        return;

                    OnInteract.Invoke();
                    WriteDefaultInteractionMessage();
                    canInteract = false;
                }
                else
                {
                    if (canInteract)
                        return;

                    OnLeaveInteract.Invoke();
                    canInteract = true;
                }
            }
            else
            {
                if (PlayerInput.Maps.Player.Interact.triggered)
                {
                    if (!canInteract)
                        return;

                    OnInteract.Invoke();
                    WriteDefaultInteractionMessage();
                    canInteract = false;

                    if (multipleInteractions && !interactionDelayCache)
                        StartCoroutine(InteractionDelay());
                }
            }
        }
        else
        {
            if (PlayerInput.Maps.Player.Interact.triggered)
            {
                if (!isHiding)
                {
                    OnInteract.Invoke();
                    WriteDefaultInteractionMessage();
                    isHiding = true;
                }
                else
                {
                    OnLeaveInteract.Invoke();
                    Debug.Log("unhide!");
                    isHiding = false;
                }
                
            }

        }
        
    }

    IEnumerator InteractionDelay(){
        interactionDelayCache = true;

        yield return new WaitForSeconds(timeBeforeNextInteraction);
        canInteract = true;
        
        interactionDelayCache = false;
    }

    void DisplayInteractionUI(){
        if(interactionPrompt == null)
            return;
        if(!interactionPrompt.activeSelf)
            interactionPrompt.SetActive(true);
    }

    public void WriteDefaultInteractionMessage(){
        Debug.Log($"<color=white>Player interacted with:</color> <color=red>" + gameObject.name + "</color>");
    }
}