using UnityEngine;

public class EnemyTagging : MonoBehaviour
{
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    // Start is called before the first frame update
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("Gadget"))
        {
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }
}
