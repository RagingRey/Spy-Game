using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class SwordMobAttack : MonoBehaviour, EnemyState
{
    public EnemyPatrol enemyPatrol;
    public Material materialDefault;
    public Material attackMaterial;
    public FieldOfView fieldOfView;
    public MeshRenderer meshRenderer;
    public NavMeshAgent navMeshAgent;
    public SwordMobHunting swordMobHunting;
    public Material chaseMaterial;
    public Animator swordAnim;

    public EnemyState RunState(){
        Debug.Log("Sword mob attack initiated!");

        if (fieldOfView.visiblePlayer.Count != 0)
        {
            if (navMeshAgent.remainingDistance - swordMobHunting.stoppingDistance <= 0)
            {
                meshRenderer.material = attackMaterial;
                StopCoroutine(swordMobHunting.followCoroutine);
                navMeshAgent.SetDestination(gameObject.transform.position);
                navMeshAgent.velocity = Vector3.zero;

                // turn on attack animation
                swordAnim.SetTrigger("slash");
                return this;
            }
            else
            {
                
                meshRenderer.material = chaseMaterial;
                return swordMobHunting;
            }

        }
        else
        {
            meshRenderer.material = materialDefault;
            return enemyPatrol;
        }
    }

    public void ExitState(){}
}
