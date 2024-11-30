using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // Событие без параметров

    private void OnEnable()
    {
        OnVictoryCanvasEnabled?.Invoke(); // Уведомляем подписчиков
        Debug.Log("VictoryCanvasEnabled активирован, событие вызвано.");
    }
}
