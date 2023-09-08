using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyStateBase
{
    #region PRIVATE VARIABLES
    public int currentPatrolPointIndex;
    public bool nextPatrolPointDecided;
    public Transform nextPatrolPoint;
    #endregion
    public override void EnterState(NewEnemyStateManager enemyStateManager)
    {
        enemyStateManager.enemyScript.Init();
        //Debug.Log("patrolStateEntered");
        enemyStateManager.enemyScript.startToPatrol = true;
        if (enemyStateManager.enemyScript.patrolPoints.Length > 0) currentPatrolPointIndex = FindTheClosestPatrolPoint(enemyStateManager);
        // Make the enemy goes to the closest patrol point
        enemyStateManager.enemyScript.StartMovingAndSetDestination(enemyStateManager.enemyScript.patrolPoints[currentPatrolPointIndex].position);
    }

    public override void UpdateState(NewEnemyStateManager enemyStateManager)
    {

        if (!enemyStateManager.enemyScript.fieldOfView.PLAYER_DETECTED)
        {
            enemyStateManager.enemyScript.startToPatrol = true;
        }
        else
        {
            // Make the enemy stands where it is
            enemyStateManager.enemyScript.startToPatrol = false;
            enemyStateManager.enemyScript.Stop();
            // Switch to the hunting state
            enemyStateManager.SwitchState(enemyStateManager.enemyHunting);
        }
    }

    private int FindTheClosestPatrolPoint(NewEnemyStateManager enemyStateManager)
    {
        float minDistance = Mathf.Infinity;
        int closestPointIndex = 0;
        for(int i = 0; i < enemyStateManager.enemyScript.patrolPoints.Length; i++)
        {
            Transform patrolPoint = enemyStateManager.enemyScript.patrolPoints[i];
            float distance = Vector3.Distance(enemyStateManager.enemyScript.transform.position, patrolPoint.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closestPointIndex = i;
            }
        }
        return closestPointIndex;
    }
}
