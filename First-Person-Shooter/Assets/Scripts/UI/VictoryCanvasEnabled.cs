using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // Событие без параметров

    public void NextButton()
    {
        OnVictoryCanvasEnabled?.Invoke(); // Уведомляем подписчиков
        Debug.Log("VictoryCanvasEnabled активирован, событие вызвано.");
    }
}
