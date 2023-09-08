using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("This is where you assign the nav mesh agent")]
    public NavMeshAgent navMeshAgent;
    [Space]
    [Tooltip("This is where you assign the mesh view object for the knocked out state")]
    public GameObject meshView;
    [Space]
    [Tooltip("This is where you assign the get up time for the knocked out state")]
    public float getUpTime;
    [Space]
    [Tooltip("How far from the destination should the agent stop")]
    [SerializeField] float reachingDestinationPaddingDistance;
    [Space]
    [Tooltip("This is where you assign the FOV script")]
    public FieldOfView fieldOfView;
    [Space]
    [Tooltip("The first patrol point in the array is where the enemy will go at the start of the game")]
    public Transform[] patrolPoints;
    [Header("Materials")]
    [Tooltip("This is where you assign the default material")]
    public Material defaultMaterial;
    [Tooltip("This is where you assign the following material")]
    public Material followMaterial;
    [Tooltip("This is where you assign the attacking material")]
    public Material attackMaterial;
    [Tooltip("This is where you assign the knocked out state material")]
    public Material knockedOutMaterial;
    [Space]
    [Tooltip("This is where you assign the attacking distance for the enemy")]
    public float attackingDistance;
    #endregion

    #region RPIVATE VARIABLES
    [HideInInspector]
    public Vector3 destinationPosition;
    [HideInInspector]
    public bool destinationArrived;
    [HideInInspector]
    public bool startToMove;
    [HideInInspector]
    public Transform currentPatrolPoint;
    [HideInInspector]
    public int currentPatrolPointIndex = 0;
    [HideInInspector]
    public bool startToPatrol;
    [HideInInspector]
    public bool startToHunt;
    [HideInInspector]
    public bool startToAttack;
    [HideInInspector]
    public bool knockedOut;
    [HideInInspector]
    public bool knockedOutConditionTrue;
    [HideInInspector]
    public bool shockTrapDetected;
    [HideInInspector]
    public bool stunGunDetected;
    [HideInInspector]
    public bool tranquilizerDetected;
    [HideInInspector]
    public float knockedOutTimer;
    #endregion

    #region ENUMOFENEMYTYPES
    public enum EnemyType {Default, Sword, Gun, Boxing};
    #endregion

    #region STARTMETHOD
    // Start is called before the first frame update
    void Start()
    {
        startToMove = false;
        destinationArrived = false;
        startToPatrol = false;
        startToHunt = false;
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
        knockedOut = false;
        knockedOutConditionTrue = false;
        knockedOutTimer = 0f;

    }
    #endregion

    #region UPDATEMETHOD
    // Update is called once per frame
    void Update()
    {
        // Move
        if (startToMove && destinationPosition != null && !destinationArrived) MoveTo(destinationPosition);
        // Check whether destination is arrived
        CheckWhetherDestinationIsArrived();
        // Patrol
        if (startToPatrol) Patrol();
        // Hunt
        if (startToHunt) FollowPlayer();
        // Attack
        if (startToAttack) Attack();
        // KnockedOut
        DetectKnockedOutCondition();
        if (knockedOut) KnockedOut();
        
    }
    #endregion

    #region METHODSTOINIT
    // Method to initialize the animator or other things
    public virtual void Init()
    {

    }
    #endregion

    #region METHODSTOMOVE
    public void MoveTo(Vector3 destination)
    {
        navMeshAgent.SetDestination(destination);
        startToMove = false;
    }

    public void CheckWhetherDestinationIsArrived()
    {
        if (navMeshAgent.remainingDistance - reachingDestinationPaddingDistance <= 0f)
        {
            destinationArrived = true;
        }    
    }

    public void StartMovingAndSetDestination(Vector3 des)
    {
        startToMove = true;
        destinationArrived = false;
        destinationPosition = des;
    }

    public virtual void Stop()
    {
        navMeshAgent.SetDestination(transform.position);
        destinationArrived = false;
        startToMove = false;
        startToAttack = false;
        startToHunt = false;
        startToPatrol = false;
        knockedOut = false;
        knockedOutConditionTrue = false;
    }

    #endregion

    #region METHODSTOATTACK
    public virtual void Attack()
    {
        gameObject.GetComponent<MeshRenderer>().material = attackMaterial;
    }
    #endregion

    #region METHODSTOPATROL
    void Patrol()
    {
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;

        if (navMeshAgent.destination == transform.position)
            navMeshAgent.destination = currentPatrolPoint.position;

        if (PatrolPointReached())
        {
            currentPatrolPoint = GetNextPatrolPoint();
            navMeshAgent.destination = currentPatrolPoint.position;
        }
    }

    bool PatrolPointReached()
    {
        return navMeshAgent.remainingDistance <= reachingDestinationPaddingDistance;
    }

    Transform GetNextPatrolPoint()
    {
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        return patrolPoints[currentPatrolPointIndex];
    }
    #endregion

    #region METHODSTOHUNT
    public void FollowPlayer()
    {
        gameObject.GetComponent<MeshRenderer>().material = followMaterial;
        foreach (Transform playerPos in fieldOfView.visiblePlayer)
        {
            Vector3 playerDes = playerPos.position;
            navMeshAgent.SetDestination(playerDes);
        }
    }
    #endregion

    #region METHODSFORKNOCKEDOUT
    public void DetectKnockedOutCondition()
    {
        if (shockTrapDetected || stunGunDetected || tranquilizerDetected) knockedOutConditionTrue = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("StunGunBullet"))
        {
            stunGunDetected = true;
            Debug.Log("StunGunBulletDetected!");
        }
        else if (other.CompareTag("TranquilizerBullet"))
        {
            tranquilizerDetected = true;
            Debug.Log("TranquilizerBulletDetected!");
        }
        else if (other.CompareTag("ShockTrap"))
        {
            shockTrapDetected = true;
        }
            
    }

    public void KnockedOut()
    {
        fieldOfView.enabled = false;
        meshView.SetActive(false);
        navMeshAgent.speed = 0;
        gameObject.GetComponent<MeshRenderer>().material = knockedOutMaterial;
        if (KnockedOutCountDownToZero()) GetUp();
    }

    public bool KnockedOutCountDownToZero()
    {
        if (knockedOutTimer < getUpTime)
        {
            knockedOutTimer += Time.deltaTime;
            return false;
        }
        else
        {
            knockedOutTimer = 0f;
            return true;
            
        }
    }

    public void GetUp()
    {
        shockTrapDetected = false;
        stunGunDetected = false;
        tranquilizerDetected = false;
        knockedOut = false;
        knockedOutConditionTrue = false;

        fieldOfView.enabled = true;
        meshView.SetActive(true);
        navMeshAgent.speed = 2;
        gameObject.GetComponent<MeshRenderer>().material = defaultMaterial;
    }
    #endregion


    public void ModifyPatrolPointsDescending()
    {
        System.Array.Reverse(patrolPoints);
    }
}
