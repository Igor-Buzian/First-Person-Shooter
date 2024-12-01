using MalbersAnimations;
using MalbersAnimations.Weapons;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Represents a slot in the inventory UI. Handles adding items, updating their quantity,
/// and invoking specific logic when the slot is clicked.
/// </summary>
public class InventorySlot : MonoBehaviour
{
    /// <summary>UI element for displaying the item's sprite.</summary>
    public Image itemImage;

    /// <summary>UI element for displaying the quantity of the item.</summary>
    public Text itemQuantityText;

    /// <summary>The current item stored in this slot.</summary>
    public InventoryLogic currentItem;

    /// <summary>Reference to the weapon manager for interacting with weapon logic.</summary>
    public MWeaponManager mWeaponManager;

    private Button slotButton;

    /// <summary>Indicates whether this slot is active.</summary>
    public bool isActive;

    Stats stats;
    private void Start()
    {
        var playerObject = GameObject.FindWithTag("Player");


        if (playerObject != null)
        {
            // Получаем компонент Stats с объекта игрока
            stats = playerObject.GetComponent<Stats>();
        }
            if (!isActive)
        {
            this.enabled = false; // Disables the script if the slot is inactive
            return;
        }

        // Check for necessary components
        slotButton = GetComponent<Button>();
        if (itemImage == null || slotButton == null)
        {
            Debug.LogError($"[InventorySlot] Missing itemImage or slotButton on {gameObject.name}.");
            gameObject.SetActive(false);
            return;
        }

        // Clears existing listeners on the button
        slotButton.onClick.RemoveAllListeners();
    }

    /// <summary>
    /// Adds an item to the slot or increases its quantity.
    /// </summary>
    /// <param name="item">The item to add or update.</param>
    public void AddItemOrIncreaseQuantity(InventoryLogic item)
    {
        if (item == null)
        {
            Debug.LogError("[AddItemOrIncreaseQuantity] The provided item is null!");
            return;
        }

        currentItem = item; // Assign the current item
        itemImage.sprite = item.spriteObject; // Set the sprite
        itemImage.enabled = true; // Show the sprite
        UpdateQuantityText();
        Debug.Log($"[AddItemOrIncreaseQuantity] Assigned item: {currentItem.Item.name}");
    }

    /// <summary>
    /// Logic to execute when the slot is clicked.
    /// Executes based on the inventory ID of the current item.
    /// </summary>
    public void OnClickLogic()
    {
        if (currentItem == null)
        {
            Debug.LogWarning($"[OnClickLogic] Current item is null in slot {gameObject.name}. Check item assignment logic.");
            return;
        }

        Debug.Log($"[OnClickLogic] Slot clicked for item: {currentItem.Item.name}");

        // Perform action based on the item's inventory ID
        switch (currentItem.inventoryID)
        {
            case 0:
            case 1:
                SetupSlotButton(currentItem.Item);
                break;
            case 2:
            case 3:
                AddBullets(currentItem.Item);
                break;
            case 4:
                HealthPlayer(currentItem.Item);
                break;
            default:
                Debug.LogWarning($"[OnClickLogic] Unknown inventoryID: {currentItem.inventoryID}");
                break;
        }
    }

    /// <summary>
    /// Adds bullets to a weapon object.
    /// </summary>
    /// <param name="weaponObject">The weapon to add bullets to.</param>
    private void AddBullets(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[AddBullets] SlotButton is not set!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[AddBullets] weaponObject is null!");
            return;
        }

        // Immediately increase ammo
        var shootable = weaponObject.GetComponent<MShootable>();
        if (shootable == null)
        {
            Debug.LogError("[AddBullets] The weaponObject lacks the MShootable component!");
            return;
        }

        shootable.TotalAmmo += 30;
        Debug.Log($"Added bullets: {shootable.TotalAmmo} for weapon {weaponObject.name}");

        // Remove the item after bullets are added
        RemoveItem();
    }

    /// <summary>
    /// Heals the player using the item.
    /// </summary>
    /// <param name="weaponObject">The object that triggers healing logic.</param>
    private void HealthPlayer(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[HealthPlayer] SlotButton is not set!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[HealthPlayer] weaponObject is null!");
            return;
        }

        // Immediately increase health
        //var stats = weaponObject.GetComponent<Stats>();
        if (stats == null)
        {
            Debug.LogError("[HealthPlayer] The weaponObject lacks the Stats component!");
            return;
        }

        var healthStat = stats.stats[0]; // Assuming stats[0] is health
        healthStat.Value = Mathf.Min(healthStat.Value + 20, 100);
        Debug.Log($"Health increased to {healthStat.Value} for object {weaponObject.name}");

        // Remove the item after healing
        RemoveItem();
    }

    /// <summary>
    /// Configures the slot button to trigger weapon-related logic.
    /// </summary>
    /// <param name="weaponObject">The weapon object to associate with the button.</param>
    private void SetupSlotButton(GameObject weaponObject)
    {
        if (slotButton == null || mWeaponManager == null)
        {
            Debug.LogError("[SetupSlotButton] SlotButton or mWeaponManager is not set!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[SetupSlotButton] weaponObject is null!");
            return;
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() =>
        {
            mWeaponManager.Holster_SetWeapon(weaponObject);
            Debug.Log($"Weapon {weaponObject.name} passed to Holster_SetWeapon.");
        });
    }

    /// <summary>
    /// Updates the text displaying the quantity of the item in the slot.
    /// </summary>
    public void UpdateQuantityText()
    {
        if (currentItem != null)
        {
            itemQuantityText.text = currentItem.quantity.ToString();
        }
        else
        {
            itemQuantityText.text = "0";
        }
    }

    /// <summary>
    /// Removes an item from the slot. Decreases its quantity or clears the slot if quantity is 0.
    /// </summary>
    public void RemoveItem()
    {
        currentItem.quantity--;
        if (currentItem.quantity <= 0)
        {
            currentItem = null;
            itemImage.enabled = false;
            itemQuantityText.text = "0";
        }
        UpdateQuantityText();
    }

    /// <summary>
    /// Checks if the slot is empty.
    /// </summary>
    /// <returns>True if the slot has no item, false otherwise.</returns>
    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
