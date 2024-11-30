using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // ������� ��� ����������

    public void NextButton()
    {
        OnVictoryCanvasEnabled?.Invoke(); // ���������� �����������
        Debug.Log("VictoryCanvasEnabled �����������, ������� �������.");
    }
}
