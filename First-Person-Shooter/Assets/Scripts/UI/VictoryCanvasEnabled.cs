using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // ������� ��� ����������

    public void SaveInventory()
    {
        OnVictoryCanvasEnabled?.Invoke(); // ���������� �����������
        PlayerPrefs.SetInt("Pass1Lvl", 1);
        Debug.Log("VictoryCanvasEnabled �����������, ������� �������.");
    }
}
