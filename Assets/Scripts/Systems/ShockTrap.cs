// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockTrap : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the ParticleSystem that contains the shockVFX")]
    [SerializeField] GameObject electricity;
    [Tooltip("Please assign the sedconds after the enemy activate the shocktrap to destroy the prefab")]
    [SerializeField] float DestroySeconds = 3;
    #endregion

    #region OTHER VARIABLES
    Rigidbody rb;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }
    #endregion

    #region DESTROY METHOD
    // Method to destroy prefab
    void Destroy()
    {
        Destroy(gameObject);
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the enemy enters the trigger zone and when the prefab touches ground
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Ground"))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
            

        }
        if (collision.CompareTag("Enemy"))
        {
            electricity.SetActive(true);
            Invoke("Destroy", DestroySeconds);

        }
    }
    #endregion  
}
