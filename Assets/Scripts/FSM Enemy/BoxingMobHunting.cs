using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class BoxingMobHunting : EnemyHunting
{
    public BoxingMobAttack boxingMobAttack;
    public float stoppingDistance = 2.5f;
    public Material materialFollow;
    public Material materialAttack;
    public IEnumerator followCoroutine;


    public override EnemyState RunState()
    {
        Debug.Log("Boxing mob Chase activated");
        List<Transform> playerInfo = new List<Transform>();
        gameObject.GetComponent<FieldOfView>().visiblePlayer.AddRange(playerInfo);

        navMeshAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
        followCoroutine = FollowPlayer(navMeshAgent);

        if (gameObject.GetComponent<FieldOfView>().visiblePlayer.Count != 0)
        {
    
            StartCoroutine(followCoroutine);
            gameObject.GetComponentInChildren<MeshRenderer>().material = materialFollow;
            if (navMeshAgent.remainingDistance - stoppingDistance <= 0)
            {
                Debug.Log("Boxing mob attack initiated!");
                StopCoroutine(followCoroutine);
                navMeshAgent.SetDestination(GameObject.FindWithTag("Player").transform.position);
                gameObject.GetComponentInChildren<MeshRenderer>().material = materialAttack;
                return boxingMobAttack;
            }
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


    public IEnumerator FollowPlayer(NavMeshAgent navMeshAgent)
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
