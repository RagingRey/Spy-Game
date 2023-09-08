// CREDITS: Medyan Mehiddine, Beyioku Daniel

using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour{
    #region INSPECTOR VARIABLES
    [Header("MOVEMENT")]
    [Tooltip("Speed when the sprinting button is not pressed")]
    [SerializeField] float speed = 5f;
    [Range(0f, 1f)]
    [SerializeField] float accelerationTime = 0.15f;
    [Range(0f, 1f)]
    [SerializeField] float decelerationTime = 0.1f;
    [Space]
    [SerializeField] float runSpeed = 10f;
    [Range(0f, 1f)] [Tooltip("Accelerates from current speed to Run Speed in this time")] 
    [SerializeField] float accelerationTimeRunning = 0.15f;
    
    [Space]
    [Header("JUMPING")]
    [Tooltip("Height from the main object transform. Jump will actually reach where player bottom is specified. "+
             "(Enable \"Debug Jump\" to see jump height)")]
    [SerializeField] float jumpHeight = 5f;
    [Tooltip("We add the jump force at the center of the player. " +
             "So we account for it by adding the difference " +
             "between the center and the bottom to the Jump Height")]
    [SerializeField] Transform playerBottom;

    [Space]
    [Header("SLOPES")]
    [Tooltip("The slope angle is the angle between the up direction and the slope")]
    [SerializeField] float maxSlopeAngle = 45f;
    [Tooltip("When the player goes on a slope above the maximum angle, they will slide with this speed")]
    [SerializeField] float slideSpeed = 20f;
    [Range(0f, 1f)]
    [SerializeField] float accelerationTimeSlide = 0.15f;
    [Tooltip("This ray will detect slopes. (Enable \"Draw Slope Ray\" to see the length)")]
    [SerializeField] float slopeRayLength = 0.25f;
    [Tooltip("Points from where the rays to detect slopes are casted. (Make sure that they are not position below the collider)")]
    [SerializeField] Transform[] slopeRaycasts;

    [Space]
    [Header("MISC OPTIONS")]
    [Tooltip("Multiplies the falling speed with this value")]
    [SerializeField] float fallingSpeedMultiplier = 1f;
    [SerializeField] float maxFallingSpeed = -9.81f;
    [Space]
    [Tooltip("Half of the size of the grounded box in each side. (Grounded box will detected when the player is grounded)")]
    [SerializeField] Vector3 groundedBoxHalfExtents;
    [Tooltip("Layers that the player will consider as ground.")]
    [SerializeField] LayerMask obstacleLayers;  

    [Space]
    [Header("REQUIRED COMPONENTS")]
    [SerializeField] Rigidbody rigidBody;
    [SerializeField] Transform groundedBoxTransform;

    [Space]
    [Header("DEBUGGING")]
    [SerializeField] bool debugging;
    [Space]
    [Tooltip("Displays the jump height")]
    [SerializeField] bool drawJumpHeight;
    [Tooltip("Writes in console the status of grounded")]
    [SerializeField] bool debugGrounded;
    [Tooltip("Displays the area where that detects the ground")]
    [SerializeField] bool drawGroundedBox;
    [Tooltip("Writes in console angle with the ground")]
    [SerializeField] bool debugAngleWithGround;
    [Tooltip("Writes in console the status of if the player is on a slope")]
    [SerializeField] bool debugOnSlope;
    [Tooltip("Displays the rays that detect slopes")]
    [SerializeField] bool drawSlopeRays;
    [Tooltip("Writes in console the velocity vector and its magnitude")]
    [SerializeField] bool debugVelocity;
    [Tooltip("Draws a ray from the center of the player with the direction of movement with input. (Except jumping)")]
    [SerializeField] bool drawMovementDirection;

    //Grappling variables (CREDITS: Beyioku Daniel)
    [Header("Grappling")]
    [Tooltip("Used to freeze player movement")]
    public bool freeze;
    [Tooltip("Used to stop movement when grappling")]
    public bool activeGrapple;
    //XRay variables
    [Tooltip("Used to stop movement during usage of XRay")]
    public bool xRayActive;


    [Tooltip("To determine if player is hiding or not")]
    public bool isHiding;

    #endregion

    #region PRIVATE & HIDDEN VARIABLES
    float jumpInitialVelocity; // Depends on the gravity and jump height set in the inspector
    float slopeAngle;
    Vector3 velocity = Vector2.zero;    
    Vector3 movementDirectionOnGround = Vector3.zero;
    RaycastHit slopeHit;

    // STATES
    bool isGrounded;
    bool isOnSlope;

    // INPUT
    bool runInputReceived;

    // CACHE (used to stop IEnumerators from executing mutltiple times)
    bool jumpCache;

    public Climb climb;
    bool jumping;

    // REFERENCES (used in SmoothDamp())
    float refVelocityX = 0f;
    float refVelocityY = 0f;
    float refVelocityZ = 0f;

    //Grappling variables (CREDITS: Beyioku Daniel)
    [Header("Grappling")]
    private Vector3 velocityToSet;
    private bool enableMovementOnNextTouch;
    #endregion

    #region GETTERS & SETTERS
    public bool Grounded{ get{return isGrounded;} }
    public bool OnSlope{ get{return isOnSlope;} }
    public bool Jumping { get { return jumping; } }
    public LayerMask ObstacleLayer { get { return obstacleLayers; } }
    public Vector3 getMoveDir()
    {
        return movementDirectionOnGround;
    }
    #endregion

    #region EXCECUTION
    void Awake() {
        UpdateJumpVelocity();

        // Better collision detection (Fixes bounciness on collision)
        if(rigidBody.interpolation != RigidbodyInterpolation.Interpolate)
            rigidBody.interpolation = RigidbodyInterpolation.Interpolate;
        if(rigidBody.collisionDetectionMode != CollisionDetectionMode.ContinuousSpeculative)
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
    }

    void Start() {
        PlayerInput.Maps.Player.Run.performed += _ => runInputReceived = true;
        PlayerInput.Maps.Player.Run.canceled += _ => runInputReceived = false;
    }

    void Update(){
        GroundCheck();
        LimitFallingVelocity();

        if (freeze)
            rigidBody.velocity = Vector3.zero;

        if (isHiding)
        {
            rigidBody.velocity = Vector3.zero;
            rigidBody.isKinematic = true;
        }
        else
        {
            rigidBody.isKinematic = false;
        }


        if (PlayerInput.Maps.Player.Jump.triggered && !jumpCache)
            StartCoroutine(Jump());

        // DEBUGGING
        if(!Logger.GlobalDebugging)
            return;
        DebugGrounded();
        DebugOnSlope();
        DebugVelocity();
        DebugJumping();
        DebugAngleWithGround();
        DrawMovementDirection(Color.magenta);
    }

    void FixedUpdate() {
        velocity.y = rigidBody.velocity.y;

        Move();    
        MultiplyFallingSpeed();
    }

    void OnDrawGizmos() {
        // DEBUGGING
        if(!Logger.GlobalDebugging)
            return;
        if(debugGrounded){
            GroundCheck();
            DebugGrounded();
        }
        DebugJumping();
        DrawBox(groundedBoxTransform.position, groundedBoxHalfExtents, Color.red);    
        DrawSlopeRays(Color.magenta);    
    }
    #endregion

    #region MISC.
    void LimitFallingVelocity(){
        if(rigidBody.velocity.y < maxFallingSpeed)
            rigidBody.velocity = new Vector3(rigidBody.velocity.x, maxFallingSpeed, rigidBody.velocity.z);
    }

    void MultiplyFallingSpeed(){
        if(rigidBody.velocity.y < 0){
            rigidBody.velocity += Vector3.up * Physics.gravity.y * fallingSpeedMultiplier * Time.fixedDeltaTime;
        }
    }
    #endregion

    #region MOVMENT
    void Move(){

        SetMovementDirectionToInput();

        isOnSlope = IsOnSlope(); 
        if(isOnSlope){
            if(slopeAngle > maxSlopeAngle){
                Slide();
                return;
            }
            movementDirectionOnGround = GetSlopeDirection();
        }
        else
            movementDirectionOnGround.y = 0; // (Doesnt affect velocity) Fixes an issue in DrawMovementDirection() where the direction points downwards. Not sure why.
        
        rigidBody.useGravity = !isOnSlope; // Disabling gravity on slopes to stop sliding

        if (activeGrapple) return; //To stop movement when grappling
        if (xRayActive) { rigidBody.velocity = Vector3.zero; return; }  //To stop movement during xray

        if(PlayerInput.Horizontal != Vector2.zero)
            velocity = runInputReceived ? 
                Accelerate(velocity, runSpeed, accelerationTimeRunning, isOnSlope) : 
                Accelerate(velocity, speed, accelerationTimeRunning, isOnSlope);
        else if(PlayerInput.Horizontal == Vector2.zero && isGrounded)
            velocity = StopMovement();
        
        rigidBody.velocity = velocity;

        // Change velocity if the player is dodging
        if (gameObject.GetComponent<GoBroDodge>() != null)
        {
            if (gameObject.GetComponent<GoBroDodge>().dodgeInputReceived == true)
            {
                if (PlayerInput.Horizontal.x > 0)
                {
                    rigidBody.AddForce(transform.right * 50f, ForceMode.VelocityChange);
                }
                else if (PlayerInput.Horizontal.x < 0)
                {
                    rigidBody.AddForce(-transform.right * 50f, ForceMode.VelocityChange);
                }
                else if (PlayerInput.Horizontal.y > 0)
                {
                    rigidBody.AddForce(transform.forward * 50f, ForceMode.VelocityChange);
                }
                else if (PlayerInput.Horizontal.y < 0)
                {
                    rigidBody.AddForce(-transform.forward * 50f, ForceMode.VelocityChange);
                }

            }
        }
    }

    Vector3 Accelerate(Vector3 current, float maxSpeed, float time, bool onSlope = false){
        current.x = Mathf.SmoothDamp(current.x, maxSpeed * movementDirectionOnGround.x, ref refVelocityX, time);
        current.z = Mathf.SmoothDamp(current.z, maxSpeed * movementDirectionOnGround.z, ref refVelocityZ, time);
        if(onSlope)
            current.y = Mathf.SmoothDamp(current.y, maxSpeed * movementDirectionOnGround.y, ref refVelocityY, time);
        return current;
    }
    Vector3 Decelerate(Vector3 current, float time, bool onSlope = false){
        current.x = Mathf.SmoothDamp(current.x, 0, ref refVelocityX, time); 
        current.z = Mathf.SmoothDamp(current.z, 0, ref refVelocityZ, time);
        if(onSlope)
            current.y = Mathf.SmoothDamp(current.y, 0, ref refVelocityY, time); 
        return current;
    }

    Vector3 StopMovement(){
        if (gameObject.GetComponent<GoBroDodge>() != null){
            // Only decelerate if the player is not dodging
            if (gameObject.GetComponent<GoBroDodge>().dodgeInputReceived == false)
                return Decelerate(velocity, decelerationTime, isOnSlope);
        }
        return Decelerate(velocity, decelerationTime, isOnSlope);
    }

    bool IsOnSlope(){
        // Casts a ray from each Raycast point to detect a slope

        if(!isGrounded)
            return false;

        slopeAngle = 0f;
        foreach(Transform rayPos in slopeRaycasts){
            bool rayHit = Physics.Raycast(rayPos.position, Vector3.down, out slopeHit, 5f);
            if(rayHit){
                slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
                if(slopeAngle != 0f)
                    return true;
            }
        }

        return false;
    }

    Vector3 GetSlopeDirection(){
        return Vector3.ProjectOnPlane(movementDirectionOnGround, slopeHit.normal).normalized;
    }

    void Slide(){
        Vector3 slopeDirection = Vector3.up - slopeHit.normal * Vector3.Dot(Vector3.up,  slopeHit.normal);
        movementDirectionOnGround = -slopeDirection;
        velocity = Accelerate(velocity, slideSpeed, accelerationTimeSlide, true);
        rigidBody.velocity = velocity;
    }

    void SetMovementDirectionToInput(){
        movementDirectionOnGround.x = PlayerInput.Horizontal.x;
        movementDirectionOnGround.z = PlayerInput.Horizontal.y;
    }
    #endregion

    #region JUMPING
    IEnumerator Jump(){
        if(!isGrounded || (isOnSlope && slopeAngle > maxSlopeAngle))
            yield break;

        jumpCache = true;
        rigidBody.velocity = new Vector3(rigidBody.velocity.x, jumpInitialVelocity, rigidBody.velocity.z);

        yield return new WaitForSeconds(0.1f); // We wait to make sure that the player left the ground so we wont add force multiple times
        jumpCache = false;
    }

    public void UpdateJumpVelocity(){
        // Derived from the kinematic equations 

        float height = jumpHeight + Mathf.Abs(transform.position.y - playerBottom.position.y);
        float acceleration = rigidBody.mass * Physics.gravity.y;
        jumpInitialVelocity = Mathf.Sqrt(-2*acceleration*(height)); 
    }
    #endregion

    #region GRAPPLEJUMP
    //CREDITS: Beyioku Daniel

    //For the calculating jump during grapple
    public Vector3 CalculateJumpVelocityForGrappling(Vector3 startPoint, Vector3 endpoint, float trajectoryHeight)
    {
        float gravity = Physics.gravity.y;
        float displacementY = endpoint.y - startPoint.y;
        Vector3 displacementXZ = new Vector3(endpoint.x - startPoint.x, 0.0f, endpoint.z - startPoint.z);

        Vector3 velocityY = Vector3.up * Mathf.Sqrt(-2 * gravity * trajectoryHeight);
        Vector3 velocityXZ = displacementXZ / (Mathf.Sqrt(-2 * trajectoryHeight / gravity) +
                                               Mathf.Sqrt(2 * (displacementY - trajectoryHeight) / gravity));

        return velocityY + velocityXZ;
    }

    //For Jumping after a successful grapple
    public void JumpToPositionDuringGrapple(Vector3 targetPosition, float trajectoryHeight)
    {
        activeGrapple = true;
        velocityToSet = CalculateJumpVelocityForGrappling(transform.position, targetPosition, trajectoryHeight);
        Invoke(nameof(SetVelocity), 0.1f);
        
        Invoke(nameof(ResetRestrictions), 2f); //In-case something goes wrong
    }

    //Set velocity for jump after a successful grapple
    private void SetVelocity()
    {
        enableMovementOnNextTouch = true;
        rigidBody.velocity = velocityToSet;
    }

    private void ResetRestrictions()
    {
        activeGrapple = false;
    }

    //For enabling movement after a grapple 
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Obstacle") && enableMovementOnNextTouch)
        {
            enableMovementOnNextTouch = false;
            ResetRestrictions();

            GetComponent<Grappling>().StopGrapple();
        }
    }

    #endregion

    #region SYSTEMS
    void GroundCheck(){
        Collider[] cols = Physics.OverlapBox(groundedBoxTransform.position, groundedBoxHalfExtents, Quaternion.identity, obstacleLayers);
        isGrounded = cols.Length != 0 ? true : false;
    }
    #endregion

    #region DEBUGGING
    void DrawBox(Vector3 position, Vector3 size, Color color){
        if(!debugging || !drawGroundedBox)
            return;

        //TOP front
        Debug.DrawRay(new Vector3(position.x - size.x, position.y + size.y, position.z + size.z), new Vector3(size.x*2f, 0f, 0f), color);
        //TOP back
        Debug.DrawRay(new Vector3(position.x - size.x, position.y + size.y, position.z - size.z), new Vector3(size.x*2f, 0f, 0f), color);
        //BOTTOM front
        Debug.DrawRay(new Vector3(position.x - size.x, position.y - size.y, position.z + size.z), new Vector3(size.x*2f, 0f, 0f), color);
        //BOTTOM back
        Debug.DrawRay(new Vector3(position.x - size.x, position.y - size.y, position.z - size.z), new Vector3(size.x*2f, 0f, 0f), color);
        //LEFT front
        Debug.DrawRay(new Vector3(position.x - size.x, position.y - size.y, position.z + size.z), new Vector3(0f, size.y*2f, 0f), color);
        //LEFT back
        Debug.DrawRay(new Vector3(position.x - size.x, position.y - size.y, position.z - size.z), new Vector3(0f, size.y*2f, 0f), color);
        //RIGHT front
        Debug.DrawRay(new Vector3(position.x + size.x, position.y - size.y, position.z + size.z), new Vector3(0f, size.y*2f, 0f), color);
        //RIGHT back
        Debug.DrawRay(new Vector3(position.x + size.x, position.y - size.y, position.z - size.z), new Vector3(0f, size.y*2f, 0f), color);

        //LEFT SIDE
        //Top
        Debug.DrawRay(new Vector3(position.x - size.x, position.y + size.y, position.z - size.z), new Vector3(0f, 0f, size.z * 2f), color);
        //Bottom
        Debug.DrawRay(new Vector3(position.x - size.x, position.y - size.y, position.z - size.z), new Vector3(0f, 0f, size.z * 2f), color);
        //RIGHT SIDE
        //Top
        Debug.DrawRay(new Vector3(position.x + size.x, position.y + size.y, position.z - size.z), new Vector3(0f, 0f, size.z * 2f), color);
        //Bottom
        Debug.DrawRay(new Vector3(position.x + size.x, position.y - size.y, position.z - size.z), new Vector3(0f, 0f, size.z * 2f), color);
    }

    void DrawMovementDirection(Color color){
        if(!debugging || !drawMovementDirection)
            return;
        Debug.DrawRay(transform.position, movementDirectionOnGround * 5f, color);
    }  

    void DrawSlopeRays(Color color){
        if(!debugging || !drawSlopeRays)
            return;
        foreach(Transform ray in slopeRaycasts)
            Debug.DrawRay(ray.position, Vector3.down * slopeRayLength, color);
    }

    void DebugGrounded(){
        if(!debugging || !debugGrounded)
            return;
        if(isGrounded)
            Debug.Log($"Grounded status: <color=green>" + isGrounded + "</color>");
        else
            Debug.Log($"Grounded status: <color=red>" + isGrounded + "</color>");
    }

    void DebugOnSlope(){
        if(!debugging || !debugOnSlope)
            return;
        if(isOnSlope)
            Debug.Log($"On slope status: <color=green>" + isOnSlope + "</color>");
        else
            Debug.Log($"On Slope status: <color=red>" + isOnSlope + "</color>");
    }

    void DebugAngleWithGround(){
        if(!debugging || !debugAngleWithGround)
            return;
        Debug.Log($"Angle with ground: <color=cyan>" + slopeAngle + "</color>");
    }

    void DebugVelocity(){
        if(!debugging || !debugVelocity)
            return;
        Debug.Log($"Velocity: <color=cyan>" + rigidBody.velocity + "</color>\nSpeed: <color=red>" + rigidBody.velocity.magnitude + "</color>");
    }    

    void DebugJumping(){
        if(!debugging || !drawJumpHeight)
            return;
        
        Debug.DrawLine(new Vector3(transform.position.x - 2f, transform.position.y + jumpHeight, transform.position.z), 
                       new Vector3(transform.position.x + 2f, transform.position.y + jumpHeight, transform.position.z), Color.red);
    }
    #endregion
}
