using MalbersAnimations;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    public Image itemImage; // UI-������� ��� ����������� ������� ��������
    public Text itemQuantityText; // UI-������� ��� ����������� ���������� ���������
    public InventoryLogic currentItem; // ������� ������� � �����

    private Button slotButton;
    public MWeaponManager PlayerMWeaponManager;
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
        slotButton.onClick.AddListener(OnClickLogic);
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

        // ����������� ������ � ����������� �� ���� ��������
        Debug.Log($"[OnClickLogic] ������ ������ ��� ��������: {currentItem.Item.name}");
        switch (currentItem.inventoryID)
        {
            case 0:
                SetupSlotButton(currentItem.Item);
                break;
            case 1:
                SetupSlotButton(currentItem.Item);
                break;
            case 2:
                Debug.LogWarning("���������� ��� ID 2 �� ��������.");
                break;
            default:
                Debug.LogWarning($"[OnClickLogic] ����������� inventoryID: {currentItem.inventoryID}");
                break;
        }
    }

    private void SetupSlotButton(GameObject weaponObject)
    {
        if (slotButton == null)
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
            if (PlayerMWeaponManager != null)
            {
                PlayerMWeaponManager.Holster_SetWeapon(weaponObject);
                Debug.Log($"������ {weaponObject.name} �������� � Holster_SetWeapon.");
            }
            else
            {
                Debug.LogError("MWeaponManager �� ������ � �����!");
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
