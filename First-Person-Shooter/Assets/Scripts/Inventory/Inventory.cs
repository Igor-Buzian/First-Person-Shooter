using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using System;

public class InventoryLogic : EventArgs
{
    public int inventoryID; // Уникальный идентификатор предмета
    public Sprite spriteObject; // Спрайт предмета
    public int quantity; // Количество предметов
    public GameObject Item;
    public InventoryLogic(int id, Sprite sprite, GameObject item)
    {
        inventoryID = id;
        spriteObject = sprite;
        quantity = 1; // Начальное количество
        Item = item;
    }
}
public class Inventory : MonoBehaviour
{
    [SerializeField] public List<InventoryLogic> items = new List<InventoryLogic>(); // Список предметов в инвентаре
    public GameObject inventoryPanel; // UI-панель инвентаря
    public GameObject victoryCanvas; // Панель победы
    InventoryUI inventoryUI;
    private bool useInventory;
    private string saveFilePath;

    // Событие для обновления интерфейса
    public event System.Action<InventoryLogic> OnItemAdded;

    [Header("Player Input")]
    public MalbersInput malbersPlayerInput;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        // Подписываемся на событие VictoryCanvasEnabled
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
        // Отписываемся от события, чтобы избежать утечек памяти
        var victoryCanvasComponent = victoryCanvas?.GetComponent<VictoryCanvasEnabled>();
        if (victoryCanvasComponent != null)
        {
            victoryCanvasComponent.OnVictoryCanvasEnabled -= SaveInventory;
        }
    }

    private void Update()
    {
        // Открытие/закрытие инвентаря
        if (Input.GetKeyDown(KeyCode.I))
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
            // Проверяем, есть ли предмет с таким же ID
            if (existingItem.inventoryID == item.inventoryID)
            {
                existingItem.quantity += item.quantity; // Увеличиваем количество
                OnItemAdded?.Invoke(existingItem);     // Обновляем интерфейс
                return;
            }
        }

        // Если предмет новый, добавляем его в инвентарь
        items.Add(item);
        Debug.Log($"Added new item with ID {item.inventoryID}. Quantity: {item.quantity}");
        OnItemAdded?.Invoke(item);
    }


    public void SaveInventory()
    {
        var inventoryData = new List<InventorySlotData>();

        // Сохраняем данные всех предметов в инвентаре
        foreach (var item in items)
        {
            var slotData = new InventorySlotData
            {
                itemName = item.quantity > 0 ? item.Item.name : null, // null для пустых слотов
                inventoryID = item.quantity > 0 ? item.inventoryID : -1, // -1 для пустых слотов
                quantity = item.quantity, // Сохраняем количество, включая 0
                spritePath = item.spriteObject != null && item.quantity > 0 ? item.spriteObject.name : "" // Сохраняем путь для спрайта
            };

            inventoryData.Add(slotData);
        }

        string json = JsonUtility.ToJson(new InventoryData { slots = inventoryData }, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Инвентарь сохранен в файл: {saveFilePath}");
    }

    public void LoadInventory()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Файл сохранения инвентаря не найден!");
            return;
        }

        // Чтение JSON из файла
        string json = File.ReadAllText(saveFilePath);

        // Десериализация JSON в объект InventoryData
        InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

        if (inventoryData == null || inventoryData.slots == null)
        {
            Debug.LogWarning("Инвентарь пуст или файл поврежден.");
            return;
        }

        // Очистка текущего инвентаря
        items.Clear();

        // Воссоздание предметов в инвентаре
        foreach (var slotData in inventoryData.slots)
        {
            // Загружаем необходимые ресурсы
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
                Debug.LogWarning($"Не удалось загрузить объект с именем {slotData.itemName}");
            }
        }

        Debug.Log("Инвентарь загружен.");
    }
    public void ClearInventory()
    {
        // Очищаем список
        items.Clear();

        // Удаляем файл
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Файл инвентаря удален.");
        }

        Debug.Log("Инвентарь очищен.");
    }

}
