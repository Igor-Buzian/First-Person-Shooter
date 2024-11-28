using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]public List<InventoryLogic> items = new List<InventoryLogic>(); // Список предметов в инвентаре
    public GameObject inventoryPanel; // UI-панель инвентаря

    private bool useInventory;

    // Событие для обновления интерфейса
    public event Action<InventoryLogic> OnItemAdded;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            useInventory = !useInventory;
            inventoryPanel.SetActive(useInventory);
        }
    }

    public void AddItem(InventoryLogic item)
    {
        // Проверяем, есть ли уже предмет в инвентаре
        foreach (var existingItem in items)
        {
            if (existingItem.inventoryID == item.inventoryID)
            {
                item.quantity++; // Увеличиваем количество, если предмет уже есть

                OnItemAdded?.Invoke(item);
                return;
            }
        }
        // Если предмет новый, добавляем его в инвентарь
        items.Add(item);
        //OnItemAdded?.Invoke(item); // Уведомляем об обновлении
        Debug.Log($"Added new item with ID {item.inventoryID}. Quantity: {item.quantity}");
        OnItemAdded?.Invoke(item);
    }
}