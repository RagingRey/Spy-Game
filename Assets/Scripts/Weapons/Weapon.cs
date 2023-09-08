using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    // Properties
    public int damagePerAttack;
    public float attackCoolDownTime;
    public float timeSinceLastAttack;
    public bool attackReady;
    public bool attackConditionTrue;
    public WeaponType weaponType;

    // Weapon Type
    public enum WeaponType
    {
        Sword,
        Pistol,
        StunGun,
        TranquilizerShooter
    }
    
    // Start is called before the first frame update
    public virtual void Start()
    {
        
    }

    // Update is called once per frame
    public virtual void Update()
    {
        RunAttackTimer();
        if (attackReady && attackConditionTrue)
        {
            Attack();
            attackReady = false;
        }
    }

    public virtual void RunAttackTimer()
    {
        if (timeSinceLastAttack < attackCoolDownTime)
        {
            timeSinceLastAttack += Time.deltaTime;
        }
        else
        {
            timeSinceLastAttack = 0f;
            attackReady = true;
        }
    }
    public virtual void Attack()
    {

    }
}
