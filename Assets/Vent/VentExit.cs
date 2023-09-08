//Credits: Beyioku Daniel
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Vent
{
    public class VentExit : MonoBehaviour
    { 
        bool playerInside;
        public Transform teleportOutTo;

        [SerializeField] 
        private PlayerControls controls;

        void Start()
        { 
            playerInside = false;
            PlayerInput.Maps.Player.Ability.performed += Teleport;
        }
        
        void OnTriggerEnter()
        {
            if (CharacterSwitch.ActiveCharacter == CharacterSwitch.Scout)
                playerInside = true;
        }

        void OnTriggerExit()
        {
            if (CharacterSwitch.ActiveCharacter == CharacterSwitch.Scout)
                playerInside = false;
        }

        private void JumpAction(InputAction.CallbackContext context)
        { 
            if (context.performed) 
            { 
                // Your code to handle the jump action
            }
        }
    
        private void Teleport(InputAction.CallbackContext context) 
        { 
            Debug.Log("Test");
        
            if(playerInside) 
            { 
                GameObject.FindWithTag("Player").transform.position = teleportOutTo.transform.position;
            }
        }
    }
}
