using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyKnockedOutState : EnemyStateBase
{
    public override void EnterState(NewEnemyStateManager enemyStateManager)
    {
        enemyStateManager.enemyScript.knockedOut = true;
        if (enemyStateManager.enemyScript.GetType().Name == "GunEnemy")
        {
            //Debug.Log("GunEnemy!!!");
            enemyStateManager.enemyScript.GetComponent<GunEnemy>().gunScript.attackConditionTrue = false;
        }
    }

    public override void UpdateState(NewEnemyStateManager enemyStateManager)
    {
        if (!enemyStateManager.enemyScript.knockedOut && !enemyStateManager.enemyScript.knockedOutConditionTrue)
        {
            enemyStateManager.enemyScript.Init();
            enemyStateManager.enemyScript.Stop();
            enemyStateManager.SwitchState(enemyStateManager.enemyPatrol);
        }
        else
        {
            if (enemyStateManager.enemyScript.GetType().Name == "GunEnemy")
            {
                //Debug.Log("2GunEnemey");
                enemyStateManager.enemyScript.GetComponent<GunEnemy>().gunScript.attackConditionTrue = false;
            }
        }
    }
}
