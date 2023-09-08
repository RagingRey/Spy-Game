using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateBase
{
    public abstract void EnterState(NewEnemyStateManager enemyStateManager);

    public abstract void UpdateState(NewEnemyStateManager enemyStateManager);

}
