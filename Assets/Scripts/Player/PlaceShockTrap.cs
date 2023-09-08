// CREDITS:
// Jose Lopez

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaceShockTrap : MonoBehaviour
{
    #region INSPECTOR VARIABLES
    [Tooltip("Please assign the spawn of the shocktrap")]
    [SerializeField] GameObject spawnPoint;
    [Tooltip("Please assign the shocktrap Prefab")]
    [SerializeField] GameObject ShockTrapPrefab;
    
    [Tooltip("Please assign the number of prefabs that the player is goint to start with")]
    [SerializeField] float TrowableCount = 2;
    [Tooltip("Please assign the max number of shocktrap that the player can carry")]
    [SerializeField] float MaxTrowableCount = 2;

    [Header("Input")]
    [Tooltip("Please assign the input reference")]
    [SerializeField] InputActionReference ShockTrap;
    #endregion

    #region OTHER VARIABLES
    private float lastShoot;
    #endregion

    #region EXECUTION
    // Start is called before the first frame update
    private void Start()
    {
        lastShoot = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        MaxCount();
        SpawnTrap();
    }
    #endregion

    #region SPAWN SHOCKTRAP METHOD
    void SpawnTrap()
    {
        if (ShockTrap.action.IsPressed() && TrowableCount >= 1 && lastShoot < Time.time)
        {
            lastShoot = Time.time + 1.5f;
            Instantiate(ShockTrapPrefab, spawnPoint.transform.position, Quaternion.identity);
            TrowableCount -= 1;
        }
    }
    
    void MaxCount()
    {
        if (TrowableCount > MaxTrowableCount)
        {
            Debug.Log("Max Gadgets Reached");
            TrowableCount = MaxTrowableCount;
        }
    }
    #endregion
}
