using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public Inventory inventory; // ������ �� ���������
    public InventorySlot[] inventorySlots; // ������ ������ ���������

    private void Start()
    {
        inventory.OnItemAdded += UpdateUI; // ������������� �� �������
    }

    private void OnDisable()
    {
        inventory.OnItemAdded -= UpdateUI; // ������������ �� �������
    }
    /// <summary>
    /// Update UI logic for Inventory
    /// </summary>
    /// <param name="item"></param>
    /// <summary>
    /// ��������� ��������� ��� ���������� ��������.
    /// </summary>
    /// <param name="item">������� ��� ����������.</param>
    public void UpdateUI(InventoryLogic item)
    {
        // ���������, ���� �� �������
        foreach (var slot in inventorySlots)
        {
            if (slot.IsEmpty())
            {
                // ���� ���� ������, ��������� �������
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
            else if (slot.currentItem.inventoryID == item.inventoryID)
            {
                // ���� ���������� 0, ������� ����
                if (item.quantity <= 0)
                {
                    slot.RemoveItem(); // ��������� ������� ����
                    return;
                }

                // ���� ������� ��� ����, ��������� ����������
                slot.AddItemOrIncreaseQuantity(item);
                return;
            }
        }
    }

}