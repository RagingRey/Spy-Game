using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranquilizerShooter : Weapon
{
    // Properties
    [Header("Tranquilizer")]
    public int currentTranquilizerCount;
    public int totalTranquilizerAmount;
    public GameObject tranquilizerPrefab;
    public Transform tranquilizerPos;
    public Transform tranquilizerBottomPos;

    public static TranquilizerShooter ThisTranquilizerScript
    {
        get;
        set;
    }


    // Start is called before the first frame update
    public void Awake()
    {
        currentTranquilizerCount = totalTranquilizerAmount;
        if (ThisTranquilizerScript != null && ThisTranquilizerScript != this)
        {
            Destroy(this);
        }
        else
        {
            ThisTranquilizerScript = this;
        }
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
        if (PlayerInput.Maps.Player.Gadget.triggered)
        {
            attackConditionTrue = true;
            Debug.Log("L pressed");
        }
        else
        {
            attackConditionTrue = false;
        }

    }

    public override void Attack()
    {
        if (currentTranquilizerCount > 0 && attackReady)
        {
            if (GoBroFacingDirectionController.main.facingRight)
            {
                GameObject tranquilizer = Instantiate(tranquilizerPrefab, tranquilizerPos.position, Quaternion.Euler(90, 90, 0), transform);
            }
            else
            {
                GameObject tranquilizer = Instantiate(tranquilizerPrefab, tranquilizerPos.position, Quaternion.Euler(90, -90, 0), transform);
            }
            
            currentTranquilizerCount -= 1;
        }
    }

}
