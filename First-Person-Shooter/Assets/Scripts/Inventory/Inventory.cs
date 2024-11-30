using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<InventoryLogic> items = new List<InventoryLogic>(); // ������ ��������� � ���������
    public GameObject inventoryPanel; // UI-������ ���������
    public GameObject victoryCanvas; // ������ ������
    InventoryUI inventoryUI;
    private bool useInventory;
    private string saveFilePath;

    // ������� ��� ���������� ����������
    public event System.Action<InventoryLogic> OnItemAdded;

    [Header("Player Input")]
    public MalbersInput malbersPlayerInput;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        // ������������� �� ������� VictoryCanvasEnabled
        var victoryCanvasComponent = victoryCanvas?.GetComponent<VictoryCanvasEnabled>();
        if (victoryCanvasComponent != null)
        {
            victoryCanvasComponent.OnVictoryCanvasEnabled += SaveInventory;
        }
        LoadInventory();
        //  ClearInventory();
    }
    private void Start()
    {
        if (File.Exists(saveFilePath))
        {
            inventoryUI = FindAnyObjectByType<InventoryUI>();
            foreach (var item in items)
            {
                inventoryUI.UpdateUI(item);
            }

        }

    }

    private void OnDestroy()
    {
        // ������������ �� �������, ����� �������� ������ ������
        var victoryCanvasComponent = victoryCanvas?.GetComponent<VictoryCanvasEnabled>();
        if (victoryCanvasComponent != null)
        {
            victoryCanvasComponent.OnVictoryCanvasEnabled -= SaveInventory;
        }
    }

    private void Update()
    {
        // ��������/�������� ���������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            useInventory = !useInventory;
            if (useInventory)
            {
                Time.timeScale = 0.0001f;
                malbersPlayerInput.enabled = false;
            }
            else
            {
                Time.timeScale = 1f;
                malbersPlayerInput.enabled = true;
            }
            inventoryPanel.SetActive(useInventory);
        }
    }

    public void AddItem(InventoryLogic item)
    {
        foreach (var existingItem in items)
        {
            if (existingItem.inventoryID == item.inventoryID)
            {
                item.quantity++; // ����������� ����������
                OnItemAdded?.Invoke(item);
                return;
            }
        }

        items.Add(item); // ����� �������
        Debug.Log($"Added new item with ID {item.inventoryID}. Quantity: {item.quantity}");
        OnItemAdded?.Invoke(item);
    }

    public void SaveInventory()
    {
        var inventoryData = new List<InventorySlotData>();

        foreach (var item in items)
        {
            var slotData = new InventorySlotData
            {
                itemName = item.Item.name,
                inventoryID = item.inventoryID,
                quantity = item.quantity,
                spritePath = item.spriteObject != null ? item.spriteObject.name : ""
            };

            inventoryData.Add(slotData);
        }

        string json = JsonUtility.ToJson(new InventoryData { slots = inventoryData }, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"��������� �������� � ����: {saveFilePath}");
    }
    public void LoadInventory()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("���� ���������� ��������� �� ������!");
            return;
        }

        // ������ JSON �� �����
        string json = File.ReadAllText(saveFilePath);

        // �������������� JSON � ������ InventoryData
        InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

        if (inventoryData == null || inventoryData.slots == null)
        {
            Debug.LogWarning("��������� ���� ��� ���� ���������.");
            return;
        }

        // ������� �������� ���������
        items.Clear();

        // ����������� ��������� � ���������
        foreach (var slotData in inventoryData.slots)
        {
            // ��������� ����������� �������
            Sprite sprite = Resources.Load<Sprite>(slotData.spritePath);
            GameObject itemObject = Resources.Load<GameObject>(slotData.itemName);

            if (itemObject != null)
            {
                var item = new InventoryLogic(slotData.inventoryID, sprite, itemObject)
                {
                    quantity = slotData.quantity
                };

                items.Add(item);
            }
            else
            {
                Debug.LogWarning($"�� ������� ��������� ������ � ������ {slotData.itemName}");
            }
        }

        Debug.Log("��������� ��������.");
    }
    public void ClearInventory()
    {
        // ������� ������
        items.Clear();

        // ������� ����
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("���� ��������� ������.");
        }

        Debug.Log("��������� ������.");
    }

}
