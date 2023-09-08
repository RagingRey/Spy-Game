//Credits: Beyioku Daniel
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Inventory
{
    public class InventoryManager : MonoBehaviour
    {
        [Tooltip("This contains the items in the inventory of the player. Do not set this manually as well")]
        public List<Pickup> InventoryItems = new List<Pickup>();

        public void AddItem(Pickup item)
        {
            InventoryItems.Add(item);
        }

        public void RemoveItem(int id)
        {
            InventoryItems.RemoveAt(id);
            ResetItemId();
        }

        
        public void ResetItemId() 
        {
            //This function refreshes the Id of each item in the inventory and it's called after removing an item in the inventory
            //since removing an item (at an index) will disrupt the numbering system of the InventoryItems list and WILL throw an error
            //if an invalid index is provided.
            for (int i = 0; i < InventoryItems.Count; i++)
            {
                InventoryItems[i].Id = i;
            }
        }
    }
}
