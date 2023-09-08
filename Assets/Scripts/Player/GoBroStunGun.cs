using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBroStunGun : MonoBehaviour
{
    [SerializeField] Animator anim;
    public bool holdingGun;
    [SerializeField] Transform stunGunPosRight;
    [SerializeField] Transform stunGunPosLeft;
    [SerializeField] GameObject stunGunPrefab;
    [SerializeField] int totalNumOfStunGun;
    public int numOfStunGunUsed;
    public GameObject currentStunGun;
    // Start is called before the first frame update
    void Start()
    {
        holdingGun = false;
        numOfStunGunUsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingGun && numOfStunGunUsed < totalNumOfStunGun)
        {
            if (PlayerInput.Maps.Player.StunGun.triggered && !GetComponent<GoBroTranquilizer>().holdingShooter)
            {
                anim.SetBool("HoldStunGun", true);
                holdingGun = true;
                if (GoBroFacingDirectionController.main.facingRight)
                {
                    currentStunGun = Instantiate(stunGunPrefab, stunGunPosRight.position, Quaternion.Euler(0, 180, 0), stunGunPosRight);
                }
                else
                {
                    currentStunGun = Instantiate(stunGunPrefab, stunGunPosLeft.position, Quaternion.Euler(0, 0, 0), stunGunPosLeft);
                }
                
                numOfStunGunUsed += 1;
                
            }
        }

        if (holdingGun && currentStunGun.GetComponent<StunGun>().currentBulletCount == 0)
        {
            anim.SetBool("PutDownGun", true);
            anim.SetBool("HoldStunGun", false);
            holdingGun = false;
            StartCoroutine(DestroyGun());
        }
        
    }

    IEnumerator DestroyGun()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(currentStunGun);
        anim.SetBool("PutDownGun", false);
    }
}
