using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHuntingState : EnemyStateBase
{
    public override void EnterState(NewEnemyStateManager enemyStateManager)
    {
        //Debug.Log("hunting State Entered");
        enemyStateManager.enemyScript.startToHunt = true;
        enemyStateManager.enemyScript.Init();
    }

    public override void UpdateState(NewEnemyStateManager enemyStateManager)
    {

        //Debug.Log("hunting state updated");
        if (enemyStateManager.enemyScript.fieldOfView.visiblePlayer.Count > 0)
        {
            enemyStateManager.enemyScript.startToHunt = true;

            if (CheckAttackingCondition(enemyStateManager))
            {
                enemyStateManager.enemyScript.startToHunt = false;
                enemyStateManager.enemyScript.Stop();
                enemyStateManager.SwitchState(enemyStateManager.enemyAttacking);
            }

        }
        else
        {
            enemyStateManager.enemyScript.startToHunt = false;
            enemyStateManager.enemyScript.Stop();
            enemyStateManager.SwitchState(enemyStateManager.enemyPatrol);
        }
        
    }

    public bool CheckAttackingCondition(NewEnemyStateManager enemyStateManager)
    {
        bool canAttack = false;
        EnemyBase enemyScript = enemyStateManager.enemyScript;
        float distance = Vector3.Distance(enemyScript.transform.position, enemyScript.fieldOfView.visiblePlayer[0].transform.position);
        if (distance < enemyScript.attackingDistance) canAttack = true;
        return canAttack;

    }

    
}
