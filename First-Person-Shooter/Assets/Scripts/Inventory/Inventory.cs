using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField]public List<InventoryLogic> items = new List<InventoryLogic>(); // ������ ��������� � ���������
    public GameObject inventoryPanel; // UI-������ ���������

    private bool useInventory;

    // ������� ��� ���������� ����������
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
        // ���������, ���� �� ��� ������� � ���������
        foreach (var existingItem in items)
        {
            if (existingItem.inventoryID == item.inventoryID)
            {
                item.quantity++; // ����������� ����������, ���� ������� ��� ����

                OnItemAdded?.Invoke(item);
                return;
            }
        }
        // ���� ������� �����, ��������� ��� � ���������
        items.Add(item);
        //OnItemAdded?.Invoke(item); // ���������� �� ����������
        Debug.Log($"Added new item with ID {item.inventoryID}. Quantity: {item.quantity}");
        OnItemAdded?.Invoke(item);
    }
}