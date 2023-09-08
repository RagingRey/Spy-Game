using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBroDodge : MonoBehaviour
{
    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;
    public bool dodgeInputReceived;
    private float dodgeInputCoolDownTimer;
    private bool dodgePerformed;
    //private bool dodgeInputReceived;

    // Start is called before the first frame update
    void Start()
    {
        dodgeInputCoolDownTimer = 0f;
        dodgePerformed = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInput.Maps.Player.Dodge.triggered)
        {
            dodgeInputReceived = true;
        }

        // DodgeInput cool down timer calculation
        if (dodgeInputReceived)
        {
            if (dodgeInputCoolDownTimer < 0.25f)
            {
                dodgeInputCoolDownTimer += Time.deltaTime;
            }
            else
            {
                dodgeInputCoolDownTimer = 0f;
                dodgeInputReceived = false;
                dodgePerformed = false;
            }
        }

        if (dodgeInputReceived && !dodgePerformed)
        {
            if (PlayerInput.Horizontal.x > 0)
            {
                Debug.Log("dodge right");
                anim.SetTrigger("RightDodge");
                dodgePerformed = true;
            }
            else if (PlayerInput.Horizontal.x < 0)
            {
                Debug.Log("dodge left");
                anim.SetTrigger("LeftDodge");
                dodgePerformed = true;
            }
            else if (PlayerInput.Horizontal.y > 0)
            {
                Debug.Log("dodge forward");
                anim.SetTrigger("ForwardDodge");
                dodgePerformed = true;
            }
            else if (PlayerInput.Horizontal.y < 0)
            {
                Debug.Log("dodge backward");
                anim.SetTrigger("BackwardDodge");
                dodgePerformed = true;
            }
        }

    }
}
