// CREDITS: Medyan Mehiddine
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour{
    private static Vector2 horizontal; // Input of movement on the X and Z axis
    private static PlayerControls playerInputActions;

    #region GETTERS AND SETTERS
    // NOTE: THESE ARE USED TO GET THE VALUES OF INPUT BY OTHER CLASSES


    public static PlayerControls Maps{ get{return playerInputActions;} }
    public static Vector2 Horizontal{ get{return horizontal;} }
    #endregion

    void Awake() {
        playerInputActions = new PlayerControls();    
    }

    void OnEnable() {
        playerInputActions.Enable();    
    }

    void OnDisable() {
        playerInputActions.Disable();    
    }

    #region INPUT READING
    // NOTE: THESE ARE USED TO SET THE INPUT VARIABLES THROUGH THE PLAYER INPUT COMPONENT 

    public void ReadHorizontalInput(InputAction.CallbackContext context){
        horizontal = context.ReadValue<Vector2>();
    }
    #endregion
}
