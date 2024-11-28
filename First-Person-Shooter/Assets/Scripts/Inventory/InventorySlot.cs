using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-������� ��� ����������� ������� ��������
    public Text itemQuantityText; // UI-������� ��� ����������� ���������� ���������

    public InventoryLogic currentItem; // ������� ������� � �����
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
        itemImage.sprite = item.spriteObject; // ������������� ������
        itemImage.enabled = true; // ���������� ������
        UpdateQuantityText();
    }

    public void UpdateQuantityText()
    {
        if (currentItem != null)
        {
            itemQuantityText.text = currentItem.quantity.ToString(); // ��������� ����� ����������
        }
    }

    public void RemoveItem()
    {
        currentItem = null;
        itemImage.enabled = false; // �������� ������
        itemQuantityText.text = "0"; // ������� �����
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