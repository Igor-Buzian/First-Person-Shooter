using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-элемент для отображения спрайта предмета
    public Text itemQuantityText; // UI-элемент для отображения количества предметов

    public InventoryLogic currentItem; // Текущий предмет в слоте
    private void Start()
    {
        if(itemImage == null)
        {
            gameObject.SetActive(false);
        }
    }
    public void AddItemOrIncreaseQuantity(InventoryLogic item)
    {
        currentItem = item;
        itemImage.sprite = item.spriteObject; // Устанавливаем спрайт
        itemImage.enabled = true; // Показываем спрайт
        UpdateQuantityText();
    }

    public void UpdateQuantityText()
    {
        if (currentItem != null)
        {
            itemQuantityText.text = currentItem.quantity.ToString(); // Обновляем текст количества
        }
    }

    public void RemoveItem()
    {
        currentItem = null;
        itemImage.enabled = false; // Скрываем спрайт
        itemQuantityText.text = "0"; // Очищаем текст
    }

    public void IncreaseQuantity()
    {
        if (currentItem != null)
        {
            currentItem.quantity++;
            UpdateQuantityText();
        }
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}