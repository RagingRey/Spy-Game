using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Bullet : MonoBehaviour
{
    public float bulletSpeedScaler;
    public float destroyTimer;
    public float currentTime;
    
    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        GetComponent<Rigidbody>().AddForce(-transform.parent.transform.up * bulletSpeedScaler, ForceMode.Force);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            Destroy(gameObject);
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
