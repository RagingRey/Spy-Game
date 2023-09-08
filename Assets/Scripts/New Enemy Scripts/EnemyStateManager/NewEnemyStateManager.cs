using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewEnemyStateManager : MonoBehaviour
{
    public EnemyStateBase currentState;
    public string currentStateName;
    public EnemyPatrolState enemyPatrol = new EnemyPatrolState();
    public EnemyHuntingState enemyHunting = new EnemyHuntingState();
    public EnemyAttackingState enemyAttacking = new EnemyAttackingState();
    public EnemyKnockedOutState enemyKnockedOut = new EnemyKnockedOutState();

    #region INSPECTOR VARIABLES
    [Tooltip("This is where you assign the EnemyBase script")]
    public EnemyBase enemyScript;
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        currentState = enemyPatrol;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemyScript.knockedOutConditionTrue)
        {
            enemyScript.Stop();
            SwitchState(enemyKnockedOut);
        }
        currentState.UpdateState(this);
        currentStateName = currentState.GetType().Name;
    }

    public void SwitchState(EnemyStateBase state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
