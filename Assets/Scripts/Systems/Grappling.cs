// CREDITS:
// Beyioku Daniel

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Grappling : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Header("REFERENCES")]
    [Tooltip("Gun for shooting hooks")]
    public Transform GrappleGun;
    [Tooltip("Grapple gun position when facing LEFT direction")]
    public Transform GrappleGunPosLeft;
    [Tooltip("Grapple gun position when facing RIGHT direction")]
    public Transform GrappleGunPosRight;
    [Tooltip("Tip of the grapple gun direction when facing LEFT direction")]
    public Transform GrappleGunTipPosLeft;
    [Tooltip("Tip of the grapple gun direction when facing RIGHT direction")]
    public Transform GrappleGunTipPosRight;
    [Tooltip("Tip of the grapple gun")]
    public Transform GunTip;
    [Tooltip("Objects you can use your grapple hook on")]
    public LayerMask Grappable;
    [Tooltip("For drawing a ray showing grapple functionality")]
    public LineRenderer LineRenderer;

    [Header("GRAPPLING")]
    [Tooltip("To specify jump force during grapple")]
    public float MaxGrappleDistance;

    [Tooltip("To specify jump force during grapple")]
    public float OverShootYAxis;
    [Tooltip("Point to grapple to")]
    private Vector3 GrapplePoint;

    [Header("Cooldown")]
    [Tooltip("Time intervals within each grapple")]
    public float GrappleCooldown;

    [Header("Input")]
    public InputActionReference Grapple;

    #endregion

    #region PRIVATE VARIABLES

    private PlayerMovement _playerMovement;

    private float GrappleCooldownTimer;

    private bool _isGrappling;

    public Renderer cursor;


    #endregion

    #region EXECUTION
    void Start()
    {
        _playerMovement = GetComponent<PlayerMovement>();

        cursor.enabled = false;
    }

    void Update()
    {
        SetGrappleGunLocation();

        if(PlayerInput.Maps.Player.Grapple.IsPressed())
            StartGrapple();

        PlayerInput.Maps.Player.Grapple.canceled += ExecuteGrapple;

        if (GrappleCooldownTimer > 0) GrappleCooldownTimer -= Time.deltaTime;
    }

    void LateUpdate()
    {
        if (_isGrappling) LineRenderer.SetPosition(0, GunTip.position);
    }

    #endregion

    #region GRAPPLE FUNTIONALITY
    void StartGrapple()
    {
        if (GrappleCooldownTimer > 0) return;

        _isGrappling = true; 
        _playerMovement.freeze = true;
        
        RaycastHit hit; 
        Vector3 mousePosition = Mouse.current.position.ReadValue(); 
        mousePosition.z = Camera.main.nearClipPlane; 
        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(mousePosition);
        
        if (Physics.Raycast(mouseWorldPosition, out hit, MaxGrappleDistance, Grappable)) 
        { 
            GrapplePoint = hit.point; 
            cursor.transform.position = hit.point + Vector3.up * 0.1f; 
            cursor.enabled = true; 
        }
        else 
        {
            cursor.enabled = false;
            GrapplePoint = GrappleGun.position + mouseWorldPosition.direction * MaxGrappleDistance; 
            StopGrapple();
        }
        
        LineRenderer.enabled = true; 
        LineRenderer.SetPosition(1, GrapplePoint);
    }

    public void ExecuteGrapple(InputAction.CallbackContext context)
    {
        if(cursor.enabled)
        {
            cursor.enabled = false;
            _playerMovement.freeze = false;

            Vector3 lowestPoint = new Vector3(transform.position.x, transform.position.y - 1f, transform.position.z);

            float grapplePointRelativeYPos = GrapplePoint.y - lowestPoint.y;
            float highestPointOnArc = grapplePointRelativeYPos + OverShootYAxis;

            if (grapplePointRelativeYPos < 0) highestPointOnArc = OverShootYAxis;

            _playerMovement.JumpToPositionDuringGrapple(GrapplePoint, highestPointOnArc);

            StopGrapple();
        }
        else
        {
            StopGrapple();
        }
    }

    public void StopGrapple()
    {
        _isGrappling = false; 
        _playerMovement.freeze = false; 
        GrappleCooldownTimer = GrappleCooldown; 
        LineRenderer.enabled = false;
    }

    void SetGrappleGunLocation()
    {
        if (PlayerInput.Horizontal.x < 0)
        {
            GrappleGun.position = GrappleGunPosLeft.position;
            GunTip.position = GrappleGunTipPosLeft.position;
        }
        else if (PlayerInput.Horizontal.x > 0)
        {
            GrappleGun.position = GrappleGunPosRight.position;
            GunTip.position = GrappleGunTipPosRight.position;
        }
    }

    #endregion
}