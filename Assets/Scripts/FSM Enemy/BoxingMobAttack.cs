using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class BoxingMobAttack : MonoBehaviour, EnemyState
{
    public EnemyPatrol enemyPatrol;
    public Material materialDefault;
    public Material attackMaterial;
    public FieldOfView fieldOfView;
    public MeshRenderer meshRenderer;
    public NavMeshAgent navMeshAgent;
    public BoxingMobHunting boxingMobHunting;
    public Material chaseMaterial;
    public Animator leftFistAnim;
    public Animator rightFistAnim;

    public EnemyState RunState()
    {
        Debug.Log("Boxing mob attack initiated!");

        if (fieldOfView.visiblePlayer.Count != 0)
        {
            if (navMeshAgent.remainingDistance - boxingMobHunting.stoppingDistance <= 0)
            {
                meshRenderer.material = attackMaterial;
                StopCoroutine(boxingMobHunting.followCoroutine);
                navMeshAgent.SetDestination(gameObject.transform.position);
                navMeshAgent.velocity = Vector3.zero;

                // turn on attack animation
                leftFistAnim.SetTrigger("leftFistAttack");
                rightFistAnim.SetTrigger("rightFistAttack");
                return this;
            }
            else
            {
                meshRenderer.material = chaseMaterial;
                return boxingMobHunting;
            }

        }
        else
        {
            meshRenderer.material = materialDefault;
            return enemyPatrol;
        }

    }


}
