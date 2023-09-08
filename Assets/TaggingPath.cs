using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Assets.Scripts.FSM_Enemy;

public class TaggingPath : MonoBehaviour
{
    public bool ShowPath;
    [SerializeField] LineRenderer pathRenderer;
    [SerializeField] FieldOfView fieldOfView;

    private Transform[] patrolPoints;

    

    // Start is called before the first frame update
    void Start()
    {
        patrolPoints = gameObject.GetComponent<EnemyBase>().patrolPoints;

        ShowPath = false;
    }

    // Update is called once per frame
    void Update()
    {
        SetupPathRenderer();
        HandleTagging();
    }

    #region TAGGING
    /// <summary>
    /// Displays the patrol path if the player is seen
    /// </summary>
    void HandleTagging()
    {
        if(ShowPath == true && fieldOfView.PLAYER_DETECTED == false)
        {
            pathRenderer.enabled = true;
        }
        else
        {
            pathRenderer.enabled = false;
        }

    }

    void SetupPathRenderer()
    {
        pathRenderer.positionCount = patrolPoints.Length;

        int i = 0;
        foreach (Transform point in patrolPoints)
        {
            pathRenderer.SetPosition(i, point.position);
            i++;
        }
    }
    #endregion

    public void CollideTagging()
    {
        ShowPath = true;
    }
}
