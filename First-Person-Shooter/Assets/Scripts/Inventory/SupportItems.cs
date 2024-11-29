using System;
using UnityEngine;

public class SupportItems : MonoBehaviour
{
    public int inventoryId; // Уникальный ID предмета
    public Sprite objectSprite; // Спрайт предмета
    public Inventory inventory; // Ссылка на инвентарь
    public event EventHandler<InventoryLogic> OnItemCollected; // Событие для уведомления о сборе
    public GameObject ItemForConnect;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
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
        InventoryLogic item = new InventoryLogic(inventoryId, objectSprite, ItemForConnect);
        inventory.AddItem(item);
    }
}