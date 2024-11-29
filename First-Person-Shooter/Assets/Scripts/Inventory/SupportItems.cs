using System;
using UnityEngine;

public class SupportItems : MonoBehaviour
{
    public int inventoryId; // ���������� ID ��������
    public Sprite objectSprite; // ������ ��������
    public Inventory inventory; // ������ �� ���������
    public event EventHandler<InventoryLogic> OnItemCollected; // ������� ��� ����������� � �����
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