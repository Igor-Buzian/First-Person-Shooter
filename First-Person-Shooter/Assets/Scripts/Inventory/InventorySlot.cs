using MalbersAnimations;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-элемент для отображения спрайта предмета
    public Text itemQuantityText; // UI-элемент для отображения количества предметов
    public InventoryLogic currentItem; // Текущий предмет в слоте

    private Button slotButton;
    public MWeaponManager PlayerMWeaponManager;
    private void Start()
    {
        // Проверяем наличие необходимых компонентов
        slotButton = GetComponent<Button>();
        if (itemImage == null || slotButton == null)
        {
            Debug.LogError($"[InventorySlot] На объекте {gameObject.name} отсутствует ссылка на itemImage.");
            gameObject.SetActive(false);
            return;
        }
        // Настраиваем начальный обработчик кнопки
        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(OnClickLogic);
    }

    public void AddItemOrIncreaseQuantity(InventoryLogic item)
    {
        if (item == null)
        {
            Debug.LogError("[AddItemOrIncreaseQuantity] Переданный объект item равен null!");
            return;
        }

        currentItem = item; // Назначаем текущий предмет
        itemImage.sprite = item.spriteObject; // Устанавливаем спрайт
        itemImage.enabled = true; // Показываем спрайт
        UpdateQuantityText();
        Debug.Log($"[AddItemOrIncreaseQuantity] Назначен предмет: {currentItem.Item.name}");
    }

    public void OnClickLogic()
    {
        if (currentItem == null)
        {
            Debug.LogWarning($"[OnClickLogic] currentItem равен null в слоте {gameObject.name}. Проверьте логику назначения предметов.");
            return;
        }

        // Настраиваем кнопку в зависимости от типа предмета
        Debug.Log($"[OnClickLogic] Нажата кнопка для предмета: {currentItem.Item.name}");
        switch (currentItem.inventoryID)
        {
            case 0:
                SetupSlotButton(currentItem.Item);
                break;
            case 1:
                SetupSlotButton(currentItem.Item);
                break;
            case 2:
                Debug.LogWarning("Обработчик для ID 2 не настроен.");
                break;
            default:
                Debug.LogWarning($"[OnClickLogic] Неизвестный inventoryID: {currentItem.inventoryID}");
                break;
        }
    }

    private void SetupSlotButton(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[SetupSlotButton] SlotButton не установлен!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[SetupSlotButton] weaponObject равен null!");
            return;
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() =>
        {
            if (PlayerMWeaponManager != null)
            {
                PlayerMWeaponManager.Holster_SetWeapon(weaponObject);
                Debug.Log($"Оружие {weaponObject.name} передано в Holster_SetWeapon.");
            }
            else
            {
                Debug.LogError("MWeaponManager не найден в сцене!");
            }
        });
    }

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

    public void RemoveItem()
    {
        currentItem = null;
        itemImage.enabled = false;
        itemQuantityText.text = "0";
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
