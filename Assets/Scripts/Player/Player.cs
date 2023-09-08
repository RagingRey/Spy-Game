//Credits: Beyioku Daniel

using Assets.Scripts.Inventory;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    protected static GameObject objectInstance;

    [SerializeField]
    [Tooltip("This can be used later in the game to set a spawn checkpoint after player's death.")]
    public static Transform checkPoint;

    [SerializeField]
    [Tooltip("This is the inventory object for the player. Do not set this variable manually")]
    public InventoryManager Inventory;

    public static GameObject OBJECT_INSTANCE{
        get{
            return objectInstance;
        }
    }

    public void Awake()
    {
        if(this.GetComponent<InventoryManager>())
            Inventory = this.GetComponent<InventoryManager>();
    }

    private void Update()
    {
        objectInstance = gameObject;
    }

    #region Inventory
    public void OnTriggerEnter(Collider other)
    {
        if (Inventory != null && !other.gameObject.GetComponent<KeyID>() && other.gameObject.GetComponent<Pickup>())
        {
            Pickup pickup = other.gameObject.GetComponent<Pickup>();
            pickup.Id = Inventory.InventoryItems.Count; //This is done to track the index of an item so as to be able to remove them when needed.
            
            Inventory.AddItem(pickup); 
            Destroy(other.gameObject);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        if (Inventory != null && other.gameObject.GetComponent<KeyID>())
        {
             KeyID key = other.gameObject.GetComponent<KeyID>();
             if (other.gameObject.GetComponent<KeyID>().keyPicked) 
             { 
                 key.Id = Inventory.InventoryItems.Count; //This is done to track the index of an item so as to be able to remove them when needed.
                 Inventory.AddItem(key);
                 Destroy(other.gameObject);
             }
        }
    }
    #endregion

    #region Death

    //Write Death behaviour here
    public void Death()
    {
        GameObject gameManager = GameObject.FindGameObjectWithTag("Manager");

        if (gameManager != null)
        {
            checkPoint = gameManager.GetComponent<CharacterSwitch>().GetFirstSpawnPoint();
            this.gameObject.transform.position = checkPoint.position;

        }
    }

    #endregion
}