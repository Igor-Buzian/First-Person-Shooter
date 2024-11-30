using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // ������� ��� ����������

    private void OnEnable()
    {
        OnVictoryCanvasEnabled?.Invoke(); // ���������� �����������
        Debug.Log("VictoryCanvasEnabled �����������, ������� �������.");
    }
}
