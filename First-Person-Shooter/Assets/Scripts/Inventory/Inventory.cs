using System.IO;
using System.Collections.Generic;
using UnityEngine;
using MalbersAnimations;
using System;


public class InventoryLogic : EventArgs
{
    public int inventoryID; // Unique identifier for the item
    public Sprite spriteObject; // Sprite of the item
    public int quantity; // Quantity of items
    public GameObject Item;

    /// <summary>
    /// Initializes a new instance of the <see cref="InventoryLogic"/> class.
    /// </summary>
    public InventoryLogic(int id, Sprite sprite, GameObject item)
    {
        inventoryID = id;
        spriteObject = sprite;
        quantity = 1; // Initial quantity
        Item = item;
    }
}

public class Inventory : MonoBehaviour
{
    [SerializeField] public List<InventoryLogic> items = new List<InventoryLogic>(); // List of items in the inventory
    public GameObject inventoryPanel; // UI panel for the inventory
    public GameObject victoryCanvas; // Victory panel
    InventoryUI inventoryUI;
    private bool useInventory;
    private string saveFilePath;

    // Event for updating the interface
    public event System.Action<InventoryLogic> OnItemAdded;

    [Header("Player Input")]
    public MalbersInput malbersPlayerInput;

    private void Awake()
    {
        Time.timeScale = 1f;
        saveFilePath = Path.Combine(Application.persistentDataPath, "inventory.json");

        // Subscribe to the VictoryCanvasEnabled event
        var victoryCanvasComponent = victoryCanvas?.GetComponent<VictoryCanvasEnabled>();
        if (victoryCanvasComponent != null)
        {
            victoryCanvasComponent.OnVictoryCanvasEnabled += SaveInventory;
        }
        LoadInventory();
        // ClearInventory();
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
        // Unsubscribe from the event to prevent memory leaks
        var victoryCanvasComponent = victoryCanvas?.GetComponent<VictoryCanvasEnabled>();
        if (victoryCanvasComponent != null)
        {
            victoryCanvasComponent.OnVictoryCanvasEnabled -= SaveInventory;
        }
    }

    private void Update()
    {
        // Open/close the inventory
        if (Input.GetKeyDown(KeyCode.I))
        {
            useInventory = !useInventory;
            if (useInventory)
            {
                Time.timeScale = 0.0001f; // Pause the game
                malbersPlayerInput.enabled = false; // Disable player input
            }
            else
            {
                Time.timeScale = 1f; // Resume the game
                malbersPlayerInput.enabled = true; // Enable player input
            }
            inventoryPanel.SetActive(useInventory);
        }
    }

    /// <summary>
    /// Adds an item to the inventory.
    /// </summary>
    /// <param name="item">The item to be added.</param>
    public void AddItem(InventoryLogic item)
    {
        foreach (var existingItem in items)
        {
            // Check if the item with the same ID exists
            if (existingItem.inventoryID == item.inventoryID)
            {
                existingItem.quantity += item.quantity; // Increase quantity
                OnItemAdded?.Invoke(existingItem); // Update the interface
                return;
            }
        }

        // If the item is new, add it to the inventory
        items.Add(item);
        Debug.Log($"Added new item with ID {item.inventoryID}. Quantity: {item.quantity}");
        OnItemAdded?.Invoke(item);
    }

    /// <summary>
    /// Saves the inventory to a JSON file.
    /// </summary>
    public void SaveInventory()
    {
        var inventoryData = new List<InventorySlotData>();

        // Save data for all items in the inventory
        foreach (var item in items)
        {
            var slotData = new InventorySlotData
            {
                itemName = item.quantity > 0 ? item.Item.name : null, // null for empty slots
                inventoryID = item.quantity > 0 ? item.inventoryID : -1, // -1 for empty slots
                quantity = item.quantity, // Save quantity, including 0
                spritePath = item.spriteObject != null && item.quantity > 0 ? item.spriteObject.name : "" // Save path for sprite
            };

            inventoryData.Add(slotData);
        }

        string json = JsonUtility.ToJson(new InventoryData { slots = inventoryData }, true);
        File.WriteAllText(saveFilePath, json);

        Debug.Log($"Inventory saved to file: {saveFilePath}");
    }

    /// <summary>
    /// Loads the inventory from a JSON file.
    /// </summary>
    public void LoadInventory()
    {
        if (!File.Exists(saveFilePath))
        {
            Debug.LogWarning("Inventory save file not found!");
            return;
        }

        // Read JSON from the file
        string json = File.ReadAllText(saveFilePath);

        // Deserialize JSON into an InventoryData object
        InventoryData inventoryData = JsonUtility.FromJson<InventoryData>(json);

        if (inventoryData == null || inventoryData.slots == null)
        {
            Debug.LogWarning("Inventory is empty or the file is corrupted.");
            return;
        }

        // Clear the current inventory
        items.Clear();

        // Recreate items in the inventory
        foreach (var slotData in inventoryData.slots)
        {
            // Load necessary resources
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
                Debug.LogWarning($"Failed to load object with name {slotData.itemName}");
            }
        }

        Debug.Log("Inventory loaded.");
    }

    /// <summary>
    /// Clears the inventory and deletes the save file.
    /// </summary>
    public void ClearInventory()
    {
        // Clear the list
        items.Clear();

        // Delete the file
        if (File.Exists(saveFilePath))
        {
            File.Delete(saveFilePath);
            Debug.Log("Inventory file deleted.");
        }

        Debug.Log("Inventory cleared.");
    }
}