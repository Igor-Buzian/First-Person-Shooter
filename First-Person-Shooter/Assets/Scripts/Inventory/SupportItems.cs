using System;
using UnityEngine;

public class SupportItems : MonoBehaviour
{
    public int inventoryId; // ���������� ID ��������
    public Sprite objectSprite; // ������ ��������
    public Inventory inventory; // ������ �� ���������
    public event EventHandler<InventoryLogic> OnItemCollected; // ������� ��� ����������� � �����

    public void AddObjectInInventory()
    {
        InventoryLogic item = new InventoryLogic(inventoryId, objectSprite, gameObject);
        inventory.AddItem(item);
    }
}