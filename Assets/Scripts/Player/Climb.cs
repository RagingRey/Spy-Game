using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climb : MonoBehaviour
{
  [Space]
  [Header("CLIMBING")]
  [SerializeField] float climbReach = 0f;

  public GameObject mesh;
  public Animator animator;
  public float angleTolerance = 80f;
  public Rigidbody body;
  public PlayerMovement playerMovement;

  private float hitAngle;
  private Vector3 rotatedVector, inputDir = new Vector3 (0f,0f,0f), localMeshPosition, globalMeshPosition;
  private Quaternion rotationConstant, rotation2;
  private Ray ray;
  private RaycastHit hit, climbHit;
LayerMask obstacleLayers;
  //INPUT
  bool climbInput;
  bool IsClimb = false;
  public bool debugClimb = false;

  void Start()
    {
      //animator.enabled = false;
      rotationConstant = Quaternion.AngleAxis(90, transform.up);
      obstacleLayers = playerMovement.ObstacleLayer;
    }

  void Update()
    {
//      Debug.Log(playerMovement.Grounded + " " + playerMovement.Jumping);
      if(debugClimb)
        {
          DebugClimb();
        }
    }
  void DebugClimb()
    {
      inputDir.x = PlayerInput.Horizontal.x;
      inputDir.z = PlayerInput.Horizontal.y;

     Debug.DrawRay(transform.position + new Vector3 (0f,climbReach + 0.1f,0f) +  0.6f*inputDir ,  (climbReach+0.5f)*-transform.up, Color.blue );
            ray = new Ray(transform.position + new Vector3(0f,1f,0f), transform.up);
     Debug.DrawRay(transform.position + new Vector3(0f,1f,0f),  transform.up, Color.red );
    }

  void OnTriggerEnter(Collider other)
    {
      if(other.gameObject.layer != LayerMask.NameToLayer("Obstacle"))
        {
          return;
        }

      if(debugClimb)
        {
          //Debug.Log("Climb: Entered obstacle");
        }

      ClimbCheck();
    }

  IEnumerator DebugClimbRou(Ray ray)
    {//Debug.Log("Test");
      float i = 0;
      while(i < 10)
        {//Debug.Log("Test2");
            Debug.DrawRay(ray.origin,ray.direction*(climbReach+0.5f), Color.black);

          yield return new WaitForSeconds(Time.deltaTime);
          i += Time.deltaTime;
        }


    }
  public void ClimbCheck()
    {
      if(debugClimb)
        {
          Debug.Log("Climb: Check if can Climb");
        }

      if(!playerMovement.Jumping)
        {
          if(debugClimb)
            {
              Debug.Log("Climb Failed: Not Jumping: " + playerMovement.Jumping);
            }
          return;
        }

      if(IsClimb)
        {
          if(debugClimb)
            {
              Debug.Log("Climb Failed: Already climbing-");
            }
          return;
        }



      inputDir.x = PlayerInput.Horizontal.x;
      inputDir.z = PlayerInput.Horizontal.y;
      ray = new Ray(transform.position + new Vector3 (0f,climbReach + 0.01f,0f) +  0.6f*inputDir, -transform.up);

      if(!Physics.Raycast(ray, out climbHit, climbReach+0.5f, obstacleLayers))
        {
          if(debugClimb)
            {StartCoroutine(DebugClimbRou(ray));
              Debug.Log("Climb Failed: No surface for climbing detected.");
            }
          return;
        }

        ray = new Ray(climbHit.point, transform.up);
        Debug.Log("hit.point " + climbHit.point);
        if(Physics.Raycast(ray, out hit, 2.6f)) //Checks if there is enough room for player above climable surface.
          {
            if(debugClimb)
              {
                Debug.Log("Climb Failed: Not enough room for player to stand on climbable surface.");
              }
            return;
          }

      hitAngle = Vector3.Angle(hit.normal,  Vector3.up);

      if (hitAngle >= angleTolerance)
        {
          Debug.Log(hitAngle + "Climb Failed: Angle of detected surface is too steep");
          return;
        }


      transform.position = new Vector3(transform.position.x,climbHit.point.y -2f,transform.position.z);

      Debug.Log("Climb: Started Climb Coroutine");
      StartCoroutine(ClimbRoutine());
    }

  IEnumerator ClimbRoutine()
    {
      IsClimb = true;
      body.isKinematic = true;
      animator.enabled = true;

      localMeshPosition = mesh.transform.localPosition;

      transform.rotation = Quaternion.LookRotation(playerMovement.getMoveDir());

      animator.Play("Climb", 0, 0f);
      yield return new WaitForSeconds(0.8f);

      globalMeshPosition = mesh.transform.position;
      mesh.transform.localPosition = localMeshPosition;

      transform.position = globalMeshPosition - localMeshPosition;

      IsClimb = false;
      body.isKinematic = false;
      animator.enabled = false;
    }
}
