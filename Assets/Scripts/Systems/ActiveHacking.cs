// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;
using System;

public class ActiveHacking : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the prompt text object here")]
    [SerializeField] TextMeshProUGUI PromptText;
    [Tooltip("Please assign the seconds to change the patha again")]
    [SerializeField] float coolDown = 2;
    #endregion

    #region OTHER VARIABLES
    private bool playerNear;
    private bool HackingZone;
    private bool Activated;

    private GameObject Hacker;
    private GameObject[] Enemy;
    private EnemyBase[] enemyBase;
    private List<EnemyBase> enemyBases = new List<EnemyBase>();
    #endregion

    #region EXECUTION
    public void Awake()
    {
        HackingZone = false;
        Activated = false;
        Enemy = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject g in Enemy)
        {
            enemyBase = g.GetComponentsInChildren<EnemyBase>(true);
            enemyBases.AddRange(enemyBase);
        }
    }
    private void Update()
    {
        

        ShowPrompt();
        MakePromptDisappear();
        ChangePath();
    }
    #endregion

    #region PROMPT METHODS
    // Method to show the UI prompt
    void ShowPrompt()
    {
        if (!playerNear) return;

        if (PromptText.color.a >= 1f) return;

        float alpha = Mathf.Min(1f, PromptText.color.a + Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }

    void MakePromptDisappear()
    {
        if (playerNear) return;

        if (PromptText.color.a <= 0f) return;

        float alpha = Mathf.Max(0f, PromptText.color.a - Time.deltaTime);

        PromptText.color = new Color(PromptText.color.r, PromptText.color.g, PromptText.color.b, alpha);
    }
    #endregion

    #region CHANGE ENEMY PATH METHODS
    // Method to change direction of enemy path
    void ChangePath()
    {
        Hacker = GameObject.Find("Hacker(Clone)");

        if (HackingZone == true && Activated == false && Hacker != null && PlayerInput.Maps.Player.Interact.triggered)
        {
            Debug.Log("Interactable");
            if (enemyBase != null)
            {
                // Access and modify the array
                foreach (EnemyBase enemyBase in enemyBases)
                {
                    // Access the desired component
                    enemyBase.ModifyPatrolPointsDescending();
                    // Use the desiredComponent as needed
                }
            }
            Activated = true;
            Invoke("ActivatedBool", coolDown);

        }
    }

    void ActivatedBool()
    {
        Activated = false;
    }
    #endregion

    #region TRIGGER DETECTION
    // Method to detect whether the player enters the trigger zone
    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Hacker(Clone)")
        {
            HackingZone = true;
            playerNear = true;
        }
    }

    // Method to detect whether the player has exited the trigger zone
    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Hacker(Clone)")
        {
            HackingZone = false;
            playerNear = false;
        }
    }
    #endregion
}
