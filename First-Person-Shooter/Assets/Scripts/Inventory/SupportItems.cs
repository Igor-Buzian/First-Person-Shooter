using System;
using UnityEngine;

public class SupportItems : MonoBehaviour
{
    public int inventoryId; // Уникальный ID предмета
    public Sprite objectSprite; // Спрайт предмета
    public Inventory inventory; // Ссылка на инвентарь
    public event EventHandler<InventoryLogic> OnItemCollected; // Событие для уведомления о сборе

    public void AddObjectInInventory()
    {
        InventoryLogic item = new InventoryLogic(inventoryId, objectSprite);
        inventory.AddItem(item);
    }
}