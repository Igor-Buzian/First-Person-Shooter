using MalbersAnimations;
using MalbersAnimations.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-элемент для отображения спрайта предмета
    public Text itemQuantityText; // UI-элемент для отображения количества предметов
    public InventoryLogic currentItem; // Текущий предмет в слоте
    public MWeaponManager mWeaponManager;
    private Button slotButton;
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

        Debug.Log($"[OnClickLogic] Нажата кнопка для предмета: {currentItem.Item.name}");

        // Выполняем действие напрямую
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
                Debug.LogWarning($"[OnClickLogic] Неизвестный inventoryID: {currentItem.inventoryID}");
                break;
        }
    }

    private void AddBullets(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[AddBullets] SlotButton не установлен!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[AddBullets] weaponObject равен null!");
            return;
        }

        // Увеличиваем патроны сразу
        var shootable = weaponObject.GetComponent<MShootable>();
        if (shootable == null)
        {
            Debug.LogError("[AddBullets] У объекта weaponObject отсутствует компонент MShootable!");
            return;
        }

        shootable.TotalAmmo += 30;
        Debug.Log($"Патроны добавлены: {shootable.TotalAmmo} для оружия {weaponObject.name}");

        // Удаляем предмет после успешного добавления патронов
        RemoveItem();
    }

    private void HealthPlayer(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[HealthPlayer] SlotButton не установлен!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[HealthPlayer] weaponObject равен null!");
            return;
        }

        // Увеличиваем здоровье сразу
        var stats = weaponObject.GetComponent<Stats>();
        if (stats == null)
        {
            Debug.LogError("[HealthPlayer] У объекта weaponObject отсутствует компонент Stats!");
            return;
        }

        var healthStat = stats.stats[0]; // Предполагаем, что stats[0] — это здоровье
        healthStat.Value = Mathf.Min(healthStat.Value + 20, 100);
        Debug.Log($"Здоровье увеличено до {healthStat.Value} для объекта {weaponObject.name}");

        // Удаляем предмет после успешного увеличения здоровья
        RemoveItem();
    }



    private void SetupSlotButton(GameObject weaponObject)
    {
        if (slotButton == null || mWeaponManager == null)
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
            mWeaponManager.Holster_SetWeapon(weaponObject);
            Debug.Log($"Оружие {weaponObject.name} передано в Holster_SetWeapon.");
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
        currentItem.quantity--;
        if(currentItem.quantity <= 0)
        {
            currentItem = null;
            itemImage.enabled = false;
            itemQuantityText.text = "0";
        }
        UpdateQuantityText();
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
