using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunGun : Weapon
{
    // Properties
    [Header("Bullet")]
    public int currentBulletCount;
    public int magazineSize;
    public GameObject BulletPrefab;
    public Transform bulletPos;
    /*[Header("Reload")]
    public float reloadingTime;
    public float reloadingTimer;
    public bool isReloading;*/

    public static StunGun ThisStunGunScript
    {
        get;
        set;
    }


    // Start is called before the first frame update
    public void Awake()
    {
        currentBulletCount = magazineSize;
        if (ThisStunGunScript != null && ThisStunGunScript != this)
        {
            Destroy(this);
        }
        else
        {
            ThisStunGunScript = this;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (PlayerInput.Maps.Player.StunGun.triggered)
        {
            attackConditionTrue = true;
            Debug.Log("k pressed");
        }
        else
        {
            attackConditionTrue = false;
        }
        // StunGun doesn't have reload
        //Reload();
    }

    public override void Attack()
    {
        if (currentBulletCount > 0 && attackReady)
        {
            GameObject bullet = Instantiate(BulletPrefab, bulletPos.position, Quaternion.identity, transform);
            currentBulletCount -= 1;
        }
    }


    /*public void Reload()
    {
        if (currentBulletCount <= 0)
        {
            if (reloadingTime < reloadingTimer)
            {
                // Automatically reloading when bullet is zero
                isReloading = true;
                reloadingTime += Time.deltaTime;
            }
            else
            {
                reloadingTime = 0;
                currentBulletCount = magazineSize;
                isReloading = false;
            }
        }

    }*/
}
