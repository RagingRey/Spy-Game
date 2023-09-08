using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxingEnemy : EnemyBase
{
    [Space]
    [Header("Attacking Animation")]
    [Tooltip("This is where you assign the left fist animator")]
    [SerializeField] Animator leftFistAnim;
    [Tooltip("This is where you assign the right fist animator")]
    [SerializeField] Animator rightFistAnim;

    public override void Attack()
    {
        base.Attack();
        if (fieldOfView.visiblePlayer.Count > 0)
        {
            Vector3 dir = (fieldOfView.visiblePlayer[0].transform.position - transform.position).normalized;
            dir.y = 0f;
            transform.forward = dir;
            leftFistAnim.SetTrigger("leftFistAttack");
            rightFistAnim.SetTrigger("rightFistAttack");
        }
        
    }

    public override void Stop()
    {
        base.Stop();
        Debug.Log("Boxing Enemy Stopped!");
        leftFistAnim.enabled = false;
        rightFistAnim.enabled = false;

    }

    public override void Init()
    {
        base.Init();
        leftFistAnim.enabled = true;
        rightFistAnim.enabled = true;
    }
}
