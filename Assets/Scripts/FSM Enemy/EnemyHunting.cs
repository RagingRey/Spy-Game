using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHunting : MonoBehaviour, EnemyState
{
    public bool cantSee;
    public EnemyPatrol enemyPatrol;
    public EnemyKnockedOut enemyKnockedOut;
    public GameObject gameObject;
    public NavMeshAgent navMeshAgent;
    public Material material;
    public Material materialDefault;



    public virtual EnemyState RunState()
    {
        Debug.Log("Chase activated");
        List<Transform> playerInfo = new List<Transform>();
        gameObject.GetComponent<FieldOfView>().visiblePlayer.AddRange(playerInfo);
   

        if(gameObject.GetComponent<FieldOfView>().visiblePlayer.Count != 0)
        {
    
            navMeshAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
            StartCoroutine(FollowPlayer(navMeshAgent));
            gameObject.GetComponentInChildren<MeshRenderer>().material = material;
            if (enemyKnockedOut.KnockedOut == true)
            {
                return enemyKnockedOut;
            }
            return this;
        
        }
        
        else
        {
            gameObject.GetComponentInChildren<MeshRenderer>().material = materialDefault;
            return enemyPatrol;
        }
    }


IEnumerator FollowPlayer(NavMeshAgent navMeshAgent)
    {
      
        while(true)
        {

        foreach(Transform transform in gameObject.GetComponent<FieldOfView>().visiblePlayer)
            {
                Vector3 playerDes = transform.position;
                navMeshAgent.SetDestination(playerDes);
            }

            yield return null;
        }

    }



}
