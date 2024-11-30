using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // Ссылка на инвентарь
    public InventorySlot[] inventorySlots; // Массив слотов инвентаря

    private void Start()
    {
        inventory.OnItemAdded += UpdateUI; // Подписываемся на событие
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= UpdateUI; // Отписываемся от события
    }
    /// <summary>
    /// Update UI logic for Inventory
    /// </summary>
    /// <param name="item"></param>
    /// <summary>
    /// Обновляет интерфейс для указанного предмета.
    /// </summary>
    /// <param name="item">Предмет для обновления.</param>
    public void UpdateUI(InventoryLogic item)
    {
        // Проверяем, есть ли предмет
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                // Если слот пустой, добавляем предмет
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
            else if (slot.currentItem.inventoryID == item.inventoryID)
            {
                // Если количество 0, очищаем слот
                if (item.quantity <= 0)
                {
                    slot.RemoveItem(); // Полностью очищает слот
                    return;
                }

                // Если предмет уже есть, обновляем количество
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
        }
    }

}