// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tagging : MonoBehaviour
{
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

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerTrowable>().Mas(1);
            Destroy(gameObject);
        }
    }
    #endregion

    #region COLLISION DETECTION
    // Method to detect when the tagging collision the enemy and the player collision the tagging
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            rb.isKinematic = true;
            rb.useGravity = false;

            transform.parent = collision.transform;

            if(transform.parent == collision.transform)
            {
                collision.gameObject.GetComponent<TaggingPath>().CollideTagging();
            }
        }
        if(collision.gameObject.name == "Scout(Clone)")
        {
            collision.gameObject.GetComponent<PlayerTrowable>().Mas(1);
            Destroy(gameObject);
        }
    }
    #endregion
}


