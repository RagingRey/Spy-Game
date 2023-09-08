using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBroTranquilizer : MonoBehaviour
{
    [SerializeField] Animator anim;
    public bool holdingShooter;
    [SerializeField] Transform shooterPosRight;
    [SerializeField] Transform shooterPosLeft;
    [SerializeField] GameObject shooterPrefab;
    [SerializeField] int totalNumOfTranquilizer;
    public int numOfTranquilizerUsed;
    public GameObject currentTranquilizer;
    // Start is called before the first frame update
    void Start()
    {
        holdingShooter = false;
        numOfTranquilizerUsed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (!holdingShooter && numOfTranquilizerUsed < totalNumOfTranquilizer)
        {
            if (PlayerInput.Maps.Player.Gadget.triggered && !GetComponent<GoBroStunGun>().holdingGun)
            {
                anim.SetBool("HoldTranquilizer", true);
                holdingShooter = true;
                if (GoBroFacingDirectionController.main.facingRight)
                {
                    currentTranquilizer = Instantiate(shooterPrefab, shooterPosRight.position, Quaternion.Euler(90, 90, 0), shooterPosRight);
                }
                else
                {
                    currentTranquilizer = Instantiate(shooterPrefab, shooterPosLeft.position, Quaternion.Euler(90, -90, 0), shooterPosLeft);
                }
                numOfTranquilizerUsed += 1;

            }
        }

        if (holdingShooter && currentTranquilizer.GetComponent<TranquilizerShooter>().currentTranquilizerCount == 0)
        {
            anim.SetBool("PutDownTranquilizer", true);
            anim.SetBool("HoldTranquilizer", false);
            holdingShooter = false;
            StartCoroutine(DestroyShooter());
        }

    }

    IEnumerator DestroyShooter()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(currentTranquilizer);
        anim.SetBool("PutDownTranquilizer", false);
    }
}
