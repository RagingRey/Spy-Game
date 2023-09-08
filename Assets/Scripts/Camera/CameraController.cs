// CREDITS: 
// Medyan Mehiddine

using UnityEngine;

public class CameraController : MonoBehaviour{
    #region STRATEGIES
    [Header("Strategies")]
    [SerializeField] CameraFollow cameraFollow;
    [SerializeField] CameraLockOn cameraLockOn;
    #endregion

    CameraStrategy strategy;

    #region EXECUTION
    private void Start() {
        strategy = cameraFollow;
    }

    private void LateUpdate() {
        if(strategy != null)
            strategy.Execute();
    }

    private void Update(){

        if(EnemyLockOn.LOCKED_ENEMY != null){
            strategy = cameraLockOn;
            return;
        }
        
        strategy = cameraFollow;
    }
    #endregion
}
