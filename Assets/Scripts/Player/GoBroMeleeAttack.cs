using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBroMeleeAttack : MonoBehaviour
{
    [SerializeField] Animator anim;

    // Update is called once per frame
    void Update()
    {
        if (!gameObject.GetComponent<GoBroStunGun>().holdingGun && PlayerInput.Maps.Player.MeleeAttack.triggered)
        {
            anim.SetTrigger("MeleeAttack");
        }
        
    }
}
