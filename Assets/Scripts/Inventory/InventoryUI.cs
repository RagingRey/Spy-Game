//Credits: Beyioku Daniel
using System;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.Inventory
{
    public class InventoryUI : MonoBehaviour
    {
        //This variable references the inventory object in the player available in the scene (Every player has an Inventory)
        private InventoryManager inventory;

        [Tooltip("This is the tag of the player(s), should be typically Player. This must be set or accessing inventory will cause errors")]
        public string playerTag;

        [Header("Inventory Variables")]
        [Tooltip("The inventory container that contains the items available in the inventory. THis should be this game object this script is applied to")]
        public RectTransform InventoryContainer;
        [Tooltip("This slot is a prefab which is made foreach item in the inventory, You can find this in the prefab")]
        public Button Slot;

        [Space]

        [Header("Slot Information Properties")]
        [Tooltip("This contains information of the item selected such as name, description, images etc")]
        public Image SlotInformation;
        [Tooltip("The tag name for the image object in slot information object")]
        public string ItemInfoImageTag;
        [Tooltip("The tag name for the name object in slot information object")]
        public string ItemInfoNameTag;
        [Tooltip("The tag name for the description or item info object in slot information object")]
        public string ItemInfoDescriptionTag;
        [Tooltip("The tag name for the use button object in slot information object")]
        public string ItemInfoButtonTag;

        private bool isOpened;

        public void ListItems()
        {
            //This function gets the current player's inventory and populates their details them in the UI.

            inventory = GameObject.FindGameObjectWithTag(playerTag).GetComponent<InventoryManager>();

            if (inventory != null && !isOpened)
            {
                isOpened = true;
                foreach (var item in inventory.InventoryItems)
                {
                    Button newSlot = Instantiate(Slot);
                    newSlot.transform.SetParent(InventoryContainer.transform, false); //Makes the instantiated slot item a child of the inventory.

                    //Set up slot attributes according to the Item player picked
                    newSlot.GetComponent<Pickup>().icon = item.icon;
                    newSlot.GetComponent<Pickup>().itemName = item.itemName;
                    newSlot.GetComponent<Pickup>().itemInfo = item.itemInfo;
                    newSlot.GetComponent<Pickup>().itemType = item.itemType;
                    newSlot.GetComponent<Pickup>().Id = item.Id;

                    //This call the method "OnSlotClick" when the item is clicked on
                    newSlot.onClick.AddListener(() => OnSlotClick(newSlot.GetComponent<Pickup>()));
                }
            }
        }

        public void CloseInventory()
        {
            //This function Destroys all slots to avoid populating the inventory with same objects when reopening since the
            //"ListItems" method is called whenever the inventory is opened */

            if (inventory != null && isOpened)
            {
                isOpened = false;
                for (int i = 0; i < InventoryContainer.transform.childCount; i++)
                {
                    Destroy(InventoryContainer.transform.GetChild(i).gameObject); 
                }
            }
        }


        void OnSlotClick(Pickup item)
        {
            //This function scans all the children of the SlotInformation object and sets all necessary information for the respective children
            SlotInformation.gameObject.SetActive(true);

            for (int i = 0; i < SlotInformation.transform.childCount; i++)
            {
                if (SlotInformation.transform.GetChild(i).CompareTag(ItemInfoImageTag))
                {
                    SlotInformation.transform.GetChild(i).GetComponent<Image>().sprite = item.icon;
                    continue;
                }
                if(SlotInformation.transform.GetChild(i).CompareTag(ItemInfoNameTag))
                {
                    SlotInformation.transform.GetChild(i).GetComponent<Text>().text = item.itemName;
                    continue;
                }
                if (SlotInformation.transform.GetChild(i).CompareTag(ItemInfoDescriptionTag))
                {
                    SlotInformation.transform.GetChild(i).GetComponent<Text>().text = item.itemInfo;
                    continue;
                }
                if (SlotInformation.transform.GetChild(i).CompareTag(ItemInfoButtonTag))
                {
                    SlotInformation.transform.GetChild(i).GetComponent<Button>().onClick.RemoveAllListeners(); //This removes any function that has been tied to the use button
                                                                                                               //, since the player can click on multiple items in the inventory,
                                                                                                               //not removing added listeners will call all of them
                    SlotInformation.transform.GetChild(i).GetComponent<Button>().onClick.AddListener((() => RemoveItemAfterUse(item)));
                    continue;
                }
            }
        }

        public void RemoveItemAfterUse(Pickup item)
        {
            //This function removes an item from the inventory and updates the inventory UI
            //This function must be called when an item is used in the inventory
            item.UseItem();
            SlotInformation.gameObject.SetActive(false);

            inventory.RemoveItem(item.Id);

            CloseInventory();
            ListItems(); 
        }
    }
}
