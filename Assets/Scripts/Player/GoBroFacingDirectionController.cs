using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoBroFacingDirectionController : MonoBehaviour
{
    public bool facingRight;
    public Animator anim;

    public static GoBroFacingDirectionController main { get; private set; }

    private void Awake()
    {
        if (main != null && main != this)
        {
            Destroy(this);
        }
        else
        {
            main = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        facingRight = true;
        anim.SetBool("FaceLeft", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerInput.Horizontal.x < 0)
        {
            facingRight = false;
            anim.SetBool("FaceLeft", true);
        }
        else if (PlayerInput.Horizontal.x > 0)
        {
            facingRight = true;
            anim.SetBool("FaceLeft", false);
        }
    }
}
