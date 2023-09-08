using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class angleTest : MonoBehaviour
{
    public float angle;
    public GameObject Enemy;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = GetEnemyDirection(Enemy);
        DrawDirection(direction);
        
        Vector2 forward = new Vector2(transform.forward.x, transform.forward.z);
        float angleWithPlayerForward = Vector2.Angle(forward, direction);

        Debug.Log("Angle: " + angleWithPlayerForward);
    }

    void OnDrawGizmos()
    {
        DrawLockOnAngle();
    }

    Vector2 GetEnemyDirection(GameObject enemy){
        // We're only gonna consider the horizontal plane 
        Vector2 enemyPos = new Vector2(enemy.transform.position.x, enemy.transform.position.z);
        Vector2 playerPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 direction = enemyPos - playerPos;
        direction.Normalize();

        return direction;
    }

    void DrawLockOnAngle(){
        Vector3 startPoint = transform.position;
        float armLength = 100f;

        Vector3 arm1EndPoint = startPoint + Quaternion.AngleAxis(angle / 2, transform.up) * transform.forward * armLength;
        Vector3 arm2EndPoint = startPoint + Quaternion.AngleAxis(-angle / 2, transform.up) * transform.forward * armLength;

        Debug.DrawRay(startPoint, arm1EndPoint - startPoint, Color.red);
        Debug.DrawRay(startPoint, arm2EndPoint - startPoint, Color.red);
        Debug.DrawRay(transform.position, transform.forward*10f, Color.green);
    }

    void DrawDirection(Vector2 direction){
        Vector3 dir = new Vector3(direction.x, 0f, direction.y);
        Debug.DrawRay(transform.position, dir, Color.yellow);
    }
}
