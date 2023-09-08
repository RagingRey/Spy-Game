// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTrowable : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the firerate of the launch")]
    [SerializeField] float fireRate = 1f;
    [Tooltip("Please assign the number of prefabs that the player is goint to start with")]
    [SerializeField] float TrowableCount = 2;
    [Tooltip("Please assign the max number of prefabs that the player can carry")]
    [SerializeField] float MaxTrowableCount = 2;

    [Tooltip("Please assign the tagging rigidbody")]
    [SerializeField] Rigidbody Trowable;
    [Tooltip("Please assign the cursor renderer")]
    [SerializeField] Renderer cursor;
    [Tooltip("Please assign the spawn of the prefab")]
    [SerializeField] Transform shootPoint;
    
    [Tooltip("Please assign the line renderer od the launching")]
    [SerializeField] LineRenderer lineVisual;
    [Tooltip("Please assign the number of line segnments")]
    [SerializeField] int linesegment;

    [Tooltip("Please assign the layers that the linevisual will collide")]
    [SerializeField] LayerMask layer;

    [Header("Input")]
    [Tooltip("Please assign the input reference")]
    [SerializeField] InputActionReference Tagging;
    #endregion

    #region OTHER VARIABLES
    private float lastShoot;
    private bool launch;
    private bool launch2;
    private Camera cam;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    private void Start()
    {
        cursor.enabled = false;
        lineVisual.enabled = false;

        cam = Camera.main;
        lastShoot = Time.time;
        lineVisual.positionCount = linesegment;

        launch = false;
        launch2 = false;
    }

    // Update is called once per frame
    private void Update()
    {
        MaxCount();
        TaggingAction();
        LaunchProjectile();
    }
    #endregion

    #region INPUT METHOD
    // Method to detect keydown and up to start action
    void TaggingAction()
    {
        Tagging.action.started += context =>
        {
            launch = true;
        };
        Tagging.action.performed += context =>
        {
            launch = true;
        };

        Tagging.action.canceled += context =>
        {
            launch = false;
        };

    }
    #endregion

    #region LAUNCH METHODS
    // Method to detect posiiton of mouse and detect velocity to launch
    void LaunchProjectile()
    {
        RaycastHit hit;
        Vector3 mousePosition = Mouse.current.position.ReadValue();
        mousePosition.z = Camera.main.nearClipPlane;
        Ray mouseWorldPosition = Camera.main.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(mouseWorldPosition, out hit, 100f, layer))
        {
            
            cursor.transform.position = hit.point + new Vector3(0,1,0) * 0.1f;

            Vector3 Vo = CalculateVelocity(hit.point, shootPoint.position, 1f);

            Visualize(Vo);

                if (Tagging.action.IsPressed() && launch == true)
                {
                    cursor.enabled = true;
                    lineVisual.enabled = true;
                    launch2 = true;

                }
                if (launch2 == true && launch == false)
                {
                    if (TrowableCount >= 1)
                    {
                        if (lastShoot < Time.time)
                        {
                            lastShoot = Time.time + 1.5f;
                            Rigidbody obj = Instantiate(Trowable, shootPoint.position, Quaternion.identity);
                            obj.velocity = Vo;

                            TrowableCount -= 1;

                            cursor.enabled = false;
                            lineVisual.enabled = false;
                            launch = false;
                            launch2 = false;
                        }
                    }
                    else
                    {
                        print("no tienes mas taggings");
                        cursor.enabled = false;
                        lineVisual.enabled = false;

                    }

                    cursor.enabled = false;
                    lineVisual.enabled = false;

                }
         
        }
    }

    #region VISUALIZE METHOD
    //added final position argument to draw the last line node to the actual target
    void Visualize(Vector3 Vo)
    {
        for(int i = 0; i < linesegment; i++)
        {
            Vector3 pos = CalculatePosInTime(Vo, i / (float)linesegment);
            lineVisual.SetPosition(i, pos);
        }
    }
    #endregion

    #region VELOCITY METHOD
    // Method to calculate the velocity
    Vector3 CalculateVelocity(Vector3 target, Vector3 origin, float time)
    {
        //Define the distance x and y first
        Vector3 distance = target - origin;
        Vector3 distanceXZ = distance;
        distanceXZ.y = 0f;

        //create float the represent our distance
        float Sy = distance.y;
        float Sxz = distanceXZ.magnitude;

        float Vxz = Sxz / time;
        float Vy = Sy / time + 0.5f * Mathf.Abs(Physics.gravity.y) * time;

        Vector3 result = distanceXZ.normalized;
        result *= Vxz;
        result.y = Vy;

        return result;
    }
    #endregion

    #region POSITION METHOD
    // Method to calculate the position of the cursor on the world space
    Vector3 CalculatePosInTime(Vector3 Vo, float time)
    {
        Vector3 Vxz = Vo;
        Vxz.y = 0f;

        Vector3 result = shootPoint.position + Vo * time;
        float sY = (-0.5f * Mathf.Abs(Physics.gravity.y) * (time * time)) + (Vo.y * time) + shootPoint.position.y;

        result.y = sY;

        return result;
    }
    #endregion

    #endregion

    #region COUNT METHODS
    // Method to collect and add more objects
    public void Mas(float count)
    {
        TrowableCount += count;
    }

    // Method to know when the max count is reach
    void MaxCount()
    {
        if(TrowableCount > MaxTrowableCount)
        {
            Debug.Log("Max Gadgets Reached");
            TrowableCount = MaxTrowableCount;
        }
    }
    #endregion
}
