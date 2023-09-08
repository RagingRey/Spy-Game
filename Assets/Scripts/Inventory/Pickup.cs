//Credits: Beyioku Daniel
//Any Item that can be picked up should be derived from this script so it can be referenced, managed and used by the inventory scripts
//Any new pickup item should be added in the enum and the UseItem function should implement what it does when used.

using UnityEngine;

public enum PickupType
{
    Painkiller,
    Health,
    DoorKey
}

public class Pickup : MonoBehaviour
{
    [Header("Item Attributes")]
    [Tooltip("This determines the object type and further determines what its usage does. If you can't see the type you are looking for simply add it to the enum in this script or contact the developers")]
    public PickupType itemType;

    [Tooltip("This is the name of the pickup")]
    public string itemName;

    [Tooltip("This contains the description of the pickup, ie: what it does, how it does it")]
    public string itemInfo;

    [Tooltip("This contains a picture for the pickup")]
    public Sprite icon;

    [HideInInspector]
    //To track item when added to inventory
    //DO NOT SET THIS VARIABLE (NEVER)
    public int Id; 


    public void UseItem()
    {
        if (itemType == PickupType.Health)
        {

        }
        if (itemType == PickupType.Painkiller)
        {

        }
        if (itemType == PickupType.DoorKey)
        {

        }
    }
}
