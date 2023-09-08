using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class GunMobAttack : MonoBehaviour, EnemyState
{
    public EnemyPatrol enemyPatrol;
    public Material materialDefault;
    public Material attackMaterial;
    public FieldOfView fieldOfView;
    public MeshRenderer meshRenderer;
    public NavMeshAgent navMeshAgent;
    public GunMobHunting gunMobHunting;
    public Material chaseMaterial;

    public  EnemyState RunState()
    {
        Debug.Log("Gun mob attack initiated!");

        if (fieldOfView.visiblePlayer.Count != 0)
        {
            if (navMeshAgent.remainingDistance - gunMobHunting.stoppingDistance <= 0)
            {
                meshRenderer.material = attackMaterial;
                StopCoroutine(gunMobHunting.followCoroutine);
                navMeshAgent.SetDestination(gameObject.transform.position);
                navMeshAgent.velocity = Vector3.zero;

                // Gun Attack Ready
                Gun.thisGunScript.attackConditionTrue = true;
                return this;
            }
            else
            {
                
                meshRenderer.material = chaseMaterial;
                Gun.thisGunScript.attackConditionTrue = false;
                return gunMobHunting;
            }

        }
        else
        {
            meshRenderer.material = materialDefault;
            Gun.thisGunScript.attackConditionTrue = false;
            return enemyPatrol;
        }

    }


}
