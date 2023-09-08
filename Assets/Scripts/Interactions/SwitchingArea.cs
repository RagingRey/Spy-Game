using UnityEngine;

public class SwitchingArea : MonoBehaviour{
    [Header("DEBUGGING")]
    [SerializeField] bool debugging;
    [Space]
    [SerializeField] bool drawAreaBoundaries;
    [Space]
    [SerializeField] BoxCollider boxCollider;
    [SerializeField] SphereCollider sphereCollider;

    void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")){
            CharacterSwitch.SetSwitchArea(transform.position);
        }
    }

    void OnTriggerExit(Collider other) {
        if(other.CompareTag("Player")){
            CharacterSwitch.LeaveSwitchingArea();
        }    
    }

    void OnDrawGizmos() {
        if(!debugging)
            return;

        Gizmos.color = Color.yellow;
        
        if(!drawAreaBoundaries)
            return;

        if(sphereCollider != null)
            Gizmos.DrawWireSphere(transform.position, sphereCollider.radius);
        if(boxCollider != null)
            Gizmos.DrawWireCube(transform.position, boxCollider.size);
    }
}
