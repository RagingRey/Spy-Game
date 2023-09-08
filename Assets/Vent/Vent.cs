//Credits: Beyioku Daniel
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.Vent
{
    public class Vent : MonoBehaviour
    {
        public Animator anim;
        public Transform teleportInto;
        bool insideVentArea;

        // Start is called before the first frame update
        void Start()
        {
            insideVentArea = false;
            PlayerInput.Maps.Player.Ability.performed += Teleport;
        }

        void OnTriggerEnter()
        {
            if(CharacterSwitch.ActiveCharacter == CharacterSwitch.Scout)
            {
                anim.Play("VentOpen");
                insideVentArea = true;
            }
        }

        void OnTriggerExit() 
        {
            if (CharacterSwitch.ActiveCharacter == CharacterSwitch.Scout)
            {
                anim.Play("VentClose");
                insideVentArea = false;
            }
        }
    
        public void Teleport(InputAction.CallbackContext context) 
        { 
            if(insideVentArea) 
            { 
                GameObject.FindWithTag("Player").transform.position = teleportInto.transform.position;
            }
        }
    }
}
