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
    private void UpdateUI(InventoryLogic item)
    {
        // Найти слот для предмета
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty() || slot.currentItem.inventoryID == item.inventoryID)
            {

                slot.AddItemOrIncreaseQuantity(item); // Добавляем предмет в пустой слот
                return;
            }
/*            else if (slot.currentItem.inventoryID == item.inventoryID)
            {
                slot.IncreaseQuantity(item); // Увеличиваем количество в уже занятом слоте
                return;
            }*/
        }
    }
}