using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordEnemy : EnemyBase
{
    [Space]
    [Header("Attacking Animation")]
    [SerializeField] Animator anim;

    public override void Attack()
    {
        base.Attack();
        if (fieldOfView.visiblePlayer.Count > 0)
        {
            Vector3 dir = (fieldOfView.visiblePlayer[0].transform.position - transform.position).normalized;
            dir.y = 0f;
            transform.forward = dir;
            anim.SetTrigger("slash");
        }
        
    }

    public override void Stop()
    {
        base.Stop();
        anim.enabled = false;
    }

    public override void Init()
    {
        base.Init();
        anim.enabled = true;
    }
}
