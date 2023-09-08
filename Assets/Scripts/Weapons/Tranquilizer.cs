using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Tranquilizer : MonoBehaviour
{
    public float tranquilizerSpeedScaler;
    public float destroyTimer;
    public float currentTime;

    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        Vector3 tFrontPos = GameObject.FindObjectOfType<TranquilizerShooter>().tranquilizerPos.position;
        Vector3 tBottomPos = GameObject.FindObjectOfType<TranquilizerShooter>().tranquilizerBottomPos.position;
        Vector3 dir = (tFrontPos - tBottomPos).normalized;
        GetComponent<Rigidbody>().AddForce(dir * tranquilizerSpeedScaler, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            GetComponent<Rigidbody>().isKinematic = true;
            gameObject.transform.parent = collision.gameObject.transform;
        }
    }

    private void Update()
    {
        DestroyBulletAfterCertainTime();
    }

    public void DestroyBulletAfterCertainTime()
    {
        if (currentTime < destroyTimer)
        {
            currentTime += Time.deltaTime;
        }
        else
        {
            currentTime = 0f;
            Destroy(gameObject);
        }
    }
}
