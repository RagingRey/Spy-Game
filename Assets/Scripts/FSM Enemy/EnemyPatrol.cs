// CREDITS: Beyioku Daniel

using Assets.Scripts.FSM_Enemy;
using UnityEngine;
using UnityEngine.AI;

public class EnemyPatrol : MonoBehaviour, EnemyState{
    #region INSPECTOR VARIABLES
    [Tooltip("The first patrol point in the array is where the enemy will go at the start of the game")]
    public Transform[] patrolPoints;
    [SerializeField] bool canBeTagged = true;
    
    
    [Space]
    [Header("REQUIRED COMPONENETS")]
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] LineRenderer pathRenderer;
    
    [Space]
    [SerializeField] FieldOfView fieldOfView;
    [SerializeField] EnemyKnockedOut knockedOutState;
    [SerializeField] EnemyHunting huntingState;
    #endregion

    #region PRIVATE VARIABLES
    int currentPatrolPointIndex = 0;
    Transform currentPatrolPoint;
    #endregion

    #region STATE VARIABLES
    [HideInInspector] public bool knockedOut;
    #endregion

    #region EXECUTION
    void Start(){
        if(patrolPoints.Length == 0)
            throw new System.Exception("No patrol points assigned to " + gameObject.name);
    
        currentPatrolPoint = patrolPoints[0];

        SetupPathRenderer();
    }

    void Update(){
        HandleTagging();
    }

    void OnTriggerEnter2D(Collider2D other){
        if(other.CompareTag("ShockTrap"))
            knockedOut = true;
    }

    public EnemyState RunState(){
        if(knockedOut){
            return knockedOutState;
        }

        if(!fieldOfView.PLAYER_DETECTED){
            Patrol();
            return this;
        }

        return huntingState;
    }
    #endregion

    #region PATROLLING
    /// <summary>
    /// Sets the destination of the AI to a patrol point 
    /// and updates it to the next point if the point is already reached 
    /// </summary>
    void Patrol(){
        if(navMeshAgent.destination == transform.position)
            navMeshAgent.destination = currentPatrolPoint.position;
        
        if(PatrolPointReached()){
            currentPatrolPoint = GetNextPatrolPoint();
            navMeshAgent.destination = currentPatrolPoint.position;
        }
    }

    bool PatrolPointReached(){
        return navMeshAgent.remainingDistance <= 0.2f;
    }

    /// <summary>
    /// Gets the next patrol point in the patrol point array
    /// </summary>
    /// <returns></returns>
    Transform GetNextPatrolPoint(){
        currentPatrolPointIndex = (currentPatrolPointIndex + 1) % patrolPoints.Length;
        return patrolPoints[currentPatrolPointIndex];
    }
    #endregion

    #region TAGGING
    /// <summary>
    /// Displays the patrol path if the player is seen
    /// </summary>
    void HandleTagging(){
        if(!canBeTagged)
            return;
        
        pathRenderer.enabled = fieldOfView.PLAYER_DETECTED;
    }

    void SetupPathRenderer(){
        pathRenderer.positionCount = patrolPoints.Length;

        int i = 0;
        foreach(Transform point in patrolPoints){
            pathRenderer.SetPosition(i, point.position);
            i++;
        }
    }
    #endregion
}