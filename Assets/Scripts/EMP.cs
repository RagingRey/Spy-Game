using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class EMP : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference Eemp;

    // Emp held currently
    public List<EMPPickup> EmpHeld;

    // how long the EMP will last
    public int empTimer = 5;

    // Whether or not the EMP can be used
    public bool canUse = true;

    // Object(s) to be labeled "Electronic"
    List<GameObject> electronicObjects = new List<GameObject>();

    // enemy objects
    List<GameObject> enemies = new List<GameObject>();

    void Start()
    {
        electronicObjects.AddRange(GameObject.FindGameObjectsWithTag("Electronic"));
        enemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInput.Maps.Player.Gadget.triggered)
        {
            Emp();
        }
    }

    public void Emp()
    {
        if (EmpHeld.Count != 0)
        {
            foreach(GameObject electronicObject in electronicObjects)
            {
                electronicObject.transform.GetChild(0).gameObject.SetActive(false);
            }
       
            foreach(GameObject enemy in enemies)
            {
                enemy.GetComponentInChildren<EnemyStateManager>().enabled = false;
            }

            Debug.Log("EMP is activated");
            EmpHeld.RemoveAt(EmpHeld.Count - 1);
           
            StartCoroutine(ofLight());
        }
        else
        {
            Debug.Log("Your EMP is out of battery");
        }

    }


    IEnumerator ofLight()
    {

        yield return new WaitForSeconds(empTimer);

        foreach (GameObject electronicObject in electronicObjects)
        {
            electronicObject.transform.GetChild(0).gameObject.SetActive(true);
        }

        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponentInChildren<EnemyStateManager>().enabled = true;
        }

    }
}