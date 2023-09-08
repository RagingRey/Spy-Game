using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Player") && collision.GetComponent<EMP>())
        {
            collision.GetComponent<EMP>().EmpHeld.Add(this);
            Debug.Log("You have picked up an EMP");
            this.gameObject.SetActive(false);
            Debug.Log(collision.GetComponent<EMP>().EmpHeld.Count);
        }
    }
}