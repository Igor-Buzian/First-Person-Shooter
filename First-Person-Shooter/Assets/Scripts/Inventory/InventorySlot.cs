using MalbersAnimations;
using MalbersAnimations.Weapons;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-������� ��� ����������� ������� ��������
    public Text itemQuantityText; // UI-������� ��� ����������� ���������� ���������
    public InventoryLogic currentItem; // ������� ������� � �����
    public MWeaponManager mWeaponManager;
    private Button slotButton;
    private void Start()
    {
        // ��������� ������� ����������� �����������
        slotButton = GetComponent<Button>();
        if (itemImage == null || slotButton == null)
        {
            Debug.LogError($"[InventorySlot] �� ������� {gameObject.name} ����������� ������ �� itemImage.");
            gameObject.SetActive(false);
            return;
        }
        // ����������� ��������� ���������� ������
        slotButton.onClick.RemoveAllListeners();

    }

    public void AddItemOrIncreaseQuantity(InventoryLogic item)
    {
        if (item == null)
        {
            Debug.LogError("[AddItemOrIncreaseQuantity] ���������� ������ item ����� null!");
            return;
        }

        currentItem = item; // ��������� ������� �������
        itemImage.sprite = item.spriteObject; // ������������� ������
        itemImage.enabled = true; // ���������� ������
        UpdateQuantityText();
        Debug.Log($"[AddItemOrIncreaseQuantity] �������� �������: {currentItem.Item.name}");
    }

    public void OnClickLogic()
    {
        if (currentItem == null)
        {
            Debug.LogWarning($"[OnClickLogic] currentItem ����� null � ����� {gameObject.name}. ��������� ������ ���������� ���������.");
            return;
        }

        Debug.Log($"[OnClickLogic] ������ ������ ��� ��������: {currentItem.Item.name}");

        // ��������� �������� ��������
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
                Debug.LogWarning($"[OnClickLogic] ����������� inventoryID: {currentItem.inventoryID}");
                break;
        }
    }

    private void AddBullets(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[AddBullets] SlotButton �� ����������!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[AddBullets] weaponObject ����� null!");
            return;
        }

        // ����������� ������� �����
        var shootable = weaponObject.GetComponent<MShootable>();
        if (shootable == null)
        {
            Debug.LogError("[AddBullets] � ������� weaponObject ����������� ��������� MShootable!");
            return;
        }

        shootable.TotalAmmo += 30;
        Debug.Log($"������� ���������: {shootable.TotalAmmo} ��� ������ {weaponObject.name}");

        // ������� ������� ����� ��������� ���������� ��������
        RemoveItem();
    }

    private void HealthPlayer(GameObject weaponObject)
    {
        if (slotButton == null)
        {
            Debug.LogError("[HealthPlayer] SlotButton �� ����������!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[HealthPlayer] weaponObject ����� null!");
            return;
        }

        // ����������� �������� �����
        var stats = weaponObject.GetComponent<Stats>();
        if (stats == null)
        {
            Debug.LogError("[HealthPlayer] � ������� weaponObject ����������� ��������� Stats!");
            return;
        }

        var healthStat = stats.stats[0]; // ������������, ��� stats[0] � ��� ��������
        healthStat.Value = Mathf.Min(healthStat.Value + 20, 100);
        Debug.Log($"�������� ��������� �� {healthStat.Value} ��� ������� {weaponObject.name}");

        // ������� ������� ����� ��������� ���������� ��������
        RemoveItem();
    }



    private void SetupSlotButton(GameObject weaponObject)
    {
        if (slotButton == null || mWeaponManager == null)
        {
            Debug.LogError("[SetupSlotButton] SlotButton �� ����������!");
            return;
        }

        if (weaponObject == null)
        {
            Debug.LogError("[SetupSlotButton] weaponObject ����� null!");
            return;
        }

        slotButton.onClick.RemoveAllListeners();
        slotButton.onClick.AddListener(() =>
        {
            mWeaponManager.Holster_SetWeapon(weaponObject);
            Debug.Log($"������ {weaponObject.name} �������� � Holster_SetWeapon.");
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
