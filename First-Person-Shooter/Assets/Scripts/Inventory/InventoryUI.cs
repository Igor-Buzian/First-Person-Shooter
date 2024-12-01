using UnityEngine;

/// <summary>
/// Manages the UI for the inventory system. Updates item slots based on inventory changes.
/// </summary>
public class InventoryUI : MonoBehaviour
{
    /// <summary>Reference to the inventory system.</summary>
    public Inventory inventory;

    /// <summary>Array of UI slots to represent inventory items.</summary>
    public InventorySlot[] inventorySlots;

    /// <summary>
    /// Subscribes to the inventory's item-added event to update the UI.
    /// </summary>
    private void Start()
    {
        inventory.OnItemAdded += UpdateUI;
    }

    /// <summary>
    /// Unsubscribes from the inventory's item-added event when the UI is disabled.
    /// </summary>
    private void OnDisable()
    {
        inventory.OnItemAdded -= UpdateUI;
    }

    /// <summary>
    /// Updates the UI when an item is added or its quantity changes.
    /// Finds the appropriate slot or adds the item to an empty slot.
    /// </summary>
    /// <param name="item">The item to update in the UI.</param>
    public void UpdateUI(InventoryLogic item)
    {
        // Iterate through the inventory slots
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                // Add the item to the first empty slot
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
            else if (slot.currentItem.inventoryID == item.inventoryID)
            {
                // If the item quantity is 0, remove it from the slot
                if (item.quantity <= 0)
                {
                    slot.RemoveItem();
                    return;
                }

                // If the item exists, update its quantity
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
        }
    }
}
