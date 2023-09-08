using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{
    public Animator animator;

    // Start is called before the first frame update
    public override void Start()
    {
        animator = GetComponent<Animator>();
    }

    public override void Attack()
    {
        /*animator.SetTrigger("slash"); Animation dealt by SwordMobAttack state*/
    }


}
