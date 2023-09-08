using Assets.Scripts.Health;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class CharColorChange : MonoBehaviour
{
    // public Rigidbody rb;

    public Material mat0, mat1, mat2;


    public Collision collision;

   
    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.C))
                collision.gameObject.GetComponent<MeshRenderer>().material.color = mat1.color;
            else if (Input.GetKey(KeyCode.C))
                collision.gameObject.GetComponent<MeshRenderer>().material.color = mat0.color;
            else if (Input.GetKey(KeyCode.C))
                collision.gameObject.GetComponent<MeshRenderer>().material.color = mat2.color;

        }
    }

    public void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.gameObject.GetComponent<MeshRenderer>().material.color = mat0.color;



    }
}