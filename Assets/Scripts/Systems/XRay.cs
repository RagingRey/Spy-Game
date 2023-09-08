// CREDITS:
// Beyioku Daniel

using System.ComponentModel.Design.Serialization;
using Assets.Scripts.Systems.Fade;
using UnityEngine;
using UnityEngine.InputSystem;

public class XRay : MonoBehaviour
{
    public LayerMask DefaultLayerMask;
    public LayerMask XRayLayerMask;

    GameObject mainCameraObject;
    Camera mainCamera;

    public Camera xRayCamera; //This should only be assigned for the Players

    private bool xRayActive;

    [Header("Input")]
    public InputActionReference XRayInput;

    private PlayerMovement _playerMovement;

    private void Start()
    {
        if (xRayCamera)
        {
            _playerMovement = GetComponent<PlayerMovement>();
            xRayCamera.enabled = false;
            mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = mainCameraObject.GetComponent<Camera>();
            _playerMovement.xRayActive = false;
        }
    }

    void Update()
    {
        SetXrayCameraLocation();

        if (XRayInput.action.triggered)
        {
            if (xRayActive)
            {
                xRayActive = !xRayActive;
                int layerNum = (int)Mathf.Log(DefaultLayerMask.value, 2);
                this.gameObject.layer = layerNum;

                ActivateXrayCamera();

              //  if (this.transform.childCount > 0)
                 //   SetupLayerForChildren(this.transform, layerNum);
            }
            else
            {
                xRayActive = !xRayActive;
                int layerNum = (int)Mathf.Log(XRayLayerMask.value, 2);
                this.gameObject.layer = layerNum;

                ActivateXrayCamera();

                //if (this.transform.childCount > 0)
                 //   SetupLayerForChildren(this.transform, layerNum);
            }
        }

    }

    void SetupLayerForChildren(Transform root, int layer)
    {
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);

        foreach (var child in children)
        {
            child.gameObject.layer = layer;
        }
    }

    void ActivateXrayCamera()
    {
        if (this.gameObject.tag == "Player" && mainCamera && xRayCamera)
        {
            if (xRayActive)
            { 
                mainCamera.enabled = false;
                xRayCamera.enabled = true;
                _playerMovement.xRayActive = true;
                this.gameObject.GetComponent<FadeObjectBlockingView>().FadeInAllObjects();
            }
            else
            {
                mainCamera.enabled = true;
                xRayCamera.enabled = false;
                _playerMovement.xRayActive = false;
            }
        }
    }

    void SetXrayCameraLocation()
    {
        if (this.gameObject.tag == "Player" && mainCamera && xRayCamera)
        {
            if (PlayerInput.Horizontal.x < 0)
            {
                xRayCamera.transform.localRotation = Quaternion.Euler(0.0f, -90.0f, 0.0f);
            }
            else if (PlayerInput.Horizontal.x > 0)
            {
                xRayCamera.transform.localRotation = Quaternion.Euler(0.0f, 90.0f, 0.0f);
            }
        }
    }
}
