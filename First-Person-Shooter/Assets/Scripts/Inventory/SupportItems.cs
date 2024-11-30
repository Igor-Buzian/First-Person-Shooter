using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class InventorySlotData
{
    public string itemName;
    public int inventoryID;
    public int quantity;
    public string spritePath;
}

[System.Serializable]
public class InventoryData
{
    public List<InventorySlotData> slots = new List<InventorySlotData>();
}

public class SupportItems : MonoBehaviour
{
    public int inventoryId; // Уникальный ID предмета
    public Sprite objectSprite; // Спрайт предмета
    public Inventory inventory; // Ссылка на инвентарь
    public event EventHandler<InventoryLogic> OnItemCollected; // Событие для уведомления о сборе
    public GameObject ItemForConnect;
    InventoryLogic item;

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
                gameObject.SetActive(false);
            }
        }
    }

    public void AddObjectInInventory()
    {
        inventory.AddItem(item);
    }
}