using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryLogic : EventArgs
{
    public int inventoryID; // ���������� ������������� ��������
    public Sprite spriteObject; // ������ ��������
    public int quantity; // ���������� ���������
    public GameObject Item;
    public InventoryLogic(int id, Sprite sprite, GameObject item)
    {
        inventoryID = id;
        spriteObject = sprite;
        quantity = 1; // ��������� ����������
        Item = item;
    }
}
public class GameCharacter : MonoBehaviour
{
    public event Action<int> OnDamageTaken; // ������� ��� �����
    public event Action<int> OnHealthRestored; // ������� ��� �������������� ��������
    public event Action<int, int> OnPoisonApplied; // ������� ��� ��� (� ������ � ��������)

    [SerializeField] TextMeshProUGUI Health;
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamageTaken?.Invoke(damage); // ����� �������
        Debug.Log($"Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        Health.text = "Health:" + health.ToString();
    }
    public void TakeDamagePoison(int damage)
    {
        health -= damage;
        Debug.Log($"Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        Health.text = "Health:" + health.ToString();
    }
    public void RestoreHealth(int amount)
    {
        health += amount;
        OnHealthRestored?.Invoke(amount); // ����� �������
        Debug.Log($"Health: {health}");
        Health.text = "Health:" + health.ToString();
    }

    private void Die()
    {
        Debug.Log("Character has died!");
        SceneManager.LoadScene(0);
    }

    public int GetHealth()
    {
        return health;
    }

    public void ApplyPoison(int totalDamage, int duration)
    {
        OnPoisonApplied?.Invoke(totalDamage, duration); // �������� � ���������� ���
        StartCoroutine(PoisonCoroutine(totalDamage, duration));
    }

    private IEnumerator PoisonCoroutine(int totalDamage, float duration)
    {
        int ticks = (int)(duration); // ���������� ����� (1 ���� � �������)
        int damagePerTick = totalDamage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(1); // ���� 1 �������
            TakeDamagePoison(damagePerTick); // ������� ����
        }
    }
}



/*using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameCharacter : MonoBehaviour
{
    public event Action<int> OnDamageTaken; // ������� ��� �����
    public event Action<int> OnHealthRestored; // ������� ��� �������������� ��������
    public event Action<int, int> OnPoisonApplied; // ������� ��� ��� (� ������ � ��������)

    [SerializeField] TextMeshProUGUI Health;
    private int health = 100;

    public void TakeDamage(int damage)
    {
        health -= damage;
        OnDamageTaken?.Invoke(damage); // ����� �������
        Debug.Log($"Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        Health.text = "Health:" + health.ToString();
    }
    public void TakeDamagePoison(int damage)
    {
        health -= damage;
        Debug.Log($"Health: {health}");

        if (health <= 0)
        {
            Die();
        }
        Health.text = "Health:" + health.ToString();
    }
    public void RestoreHealth(int amount)
    {
        health += amount;
        OnHealthRestored?.Invoke(amount); // ����� �������
        Debug.Log($"Health: {health}");
        Health.text = "Health:" + health.ToString();
    }

    private void Die()
    {
        Debug.Log("Character has died!");
        SceneManager.LoadScene(0);
    }

    public int GetHealth()
    {
        return health;
    }

    public void ApplyPoison(int totalDamage, int duration)
    {
        OnPoisonApplied?.Invoke(totalDamage, duration); // �������� � ���������� ���
        StartCoroutine(PoisonCoroutine(totalDamage, duration));
    }

    private IEnumerator PoisonCoroutine(int totalDamage, float duration)
    {
        int ticks = (int)(duration); // ���������� ����� (1 ���� � �������)
        int damagePerTick = totalDamage / ticks;

        for (int i = 0; i < ticks; i++)
        {
            yield return new WaitForSeconds(1); // ���� 1 �������
            TakeDamagePoison(damagePerTick); // ������� ����
        }
    }
}*/