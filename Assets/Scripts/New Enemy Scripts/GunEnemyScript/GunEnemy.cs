using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunEnemy : EnemyBase
{
    [Space]
    [Header("Gun Script")]
    [Tooltip("This is where you assign the gun script")]
    [Space]
    public Gun gunScript;

    public override void Attack()
    {
        base.Attack();
        if (fieldOfView.visiblePlayer.Count > 0)
        {
            Vector3 dir = (fieldOfView.visiblePlayer[0].transform.position - transform.position).normalized;
            dir.y = 0f;
            transform.forward = dir;
            gunScript.attackConditionTrue = true;
        }
        
    }

    public override void Stop()
    {
        base.Stop();
        gunScript.attackConditionTrue = false;
        gunScript.enabled = false;
    }

    public override void Init()
    {
        base.Init();
        gunScript.enabled = true;
    }
}
