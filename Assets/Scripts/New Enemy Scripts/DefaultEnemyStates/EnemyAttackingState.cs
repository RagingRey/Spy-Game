using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackingState : EnemyStateBase
{
    public override void EnterState(NewEnemyStateManager enemyStateManager)
    {
        //Debug.Log("attacking state entered");
        enemyStateManager.enemyScript.Init();
        enemyStateManager.enemyScript.startToAttack = true;
    }

    public override void UpdateState(NewEnemyStateManager enemyStateManager)
    {
        //Debug.Log("attacking state updated");
        if (enemyStateManager.enemyScript.fieldOfView.visiblePlayer.Count > 0)
        {

            if (!CheckAttackingCondition(enemyStateManager))
            {
                StopAttackState(enemyStateManager);
                enemyStateManager.SwitchState(enemyStateManager.enemyHunting);
            }
            else
            {
                enemyStateManager.enemyScript.startToAttack = true;
            }
        }
        else
        {
            StopAttackState(enemyStateManager);
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

    public void StopAttackState(NewEnemyStateManager enemyStateManager)
    {
        enemyStateManager.enemyScript.startToAttack = false;
        enemyStateManager.enemyScript.Stop();
        // Turn off the gun attack if it's a gun enemy
        if (enemyStateManager.enemyScript.GetType().Name == "GunEnemy")
        {
            enemyStateManager.enemyScript.GetComponent<GunEnemy>().gunScript.attackConditionTrue = false;
        }
    }
}
