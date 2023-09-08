using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyKnockedOut : MonoBehaviour, EnemyState
{
    public bool KnockedOut;
    public float getUpTime;
    public EnemyPatrol enemyPatrol;
    public GameObject enemy;
    public FieldOfView fov;
    public GameObject meshview;
    public NavMeshAgent navMeshAgent;
    public Material material;
    public Material materialDefault;
 //   public Animator knockAnim;


    public EnemyState RunState()
    {
        enemyPatrol.GetComponent<EnemyPatrol>();
        navMeshAgent.GetComponent<NavMeshAgent>();

        if (KnockedOut == true)
        {
            fov.enabled = false;
            meshview.SetActive(false);
            navMeshAgent.speed = 0;
            enemy.GetComponentInChildren<MeshRenderer>().material = material;
            Invoke("Getup", getUpTime);
            return this;
        }
        else
        {
            enemyPatrol.knockedOut = false;
            enemy.GetComponentInChildren<MeshRenderer>().material = materialDefault;
            navMeshAgent.speed = 2;
            fov.enabled = true;
            meshview.SetActive(true);
            CancelInvoke("Getup");
            return enemyPatrol;
            
        }
        
    }

    public void Getup()
    {
        KnockedOut = false;
    }


}
