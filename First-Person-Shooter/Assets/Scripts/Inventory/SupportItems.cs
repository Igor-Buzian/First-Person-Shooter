using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents data for a single inventory slot, used for saving and loading.
/// </summary>
[System.Serializable]
public class InventorySlotData
{
    /// <summary>The name of the item in the slot.</summary>
    public string itemName;

    /// <summary>The unique ID of the item in the inventory.</summary>
    public int inventoryID;

    /// <summary>The quantity of the item in the slot.</summary>
    public int quantity;

    /// <summary>The path to the sprite resource for the item.</summary>
    public string spritePath;
}

/// <summary>
/// Represents the inventory data for saving and loading multiple slots.
/// </summary>
[System.Serializable]
public class InventoryData
{
    /// <summary>A list of slots representing the inventory's current state.</summary>
    public List<InventorySlotData> slots = new List<InventorySlotData>();
}

/// <summary>
/// Handles collectible items that can be added to the player's inventory.
/// </summary>
public class SupportItems : MonoBehaviour
{
    /// <summary>The unique ID of the item.</summary>
    public int inventoryId;

    /// <summary>The sprite representing the item visually.</summary>
    public Sprite objectSprite;

    /// <summary>Reference to the player's inventory.</summary>
    public Inventory inventory;

    /// <summary>Event triggered when the item is collected.</summary>
    public event EventHandler<InventoryLogic> OnItemCollected;

    /// <summary>The game object associated with this item for inventory logic.</summary>
    public GameObject ItemForConnect;

    private InventoryLogic item;


    private void Start()
    {
        item = new InventoryLogic(inventoryId, objectSprite, ItemForConnect);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (inventoryId > 1)
            {
                AddObjectInInventory();
                gameObject.SetActive(false); // Deactivates the object after collection
            }
        }
    }

    /// <summary>
    /// Adds the item to the player's inventory.
    /// </summary>
    public void AddObjectInInventory()
    {
        inventory.AddItem(item);
    }
}
