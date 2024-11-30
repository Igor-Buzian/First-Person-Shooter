using System;
using UnityEngine;

public class VictoryCanvasEnabled : MonoBehaviour
{
    public event Action OnVictoryCanvasEnabled; // Событие без параметров

    public void NextButton()
    {
        OnVictoryCanvasEnabled?.Invoke(); // Уведомляем подписчиков
        PlayerPrefs.SetInt("Pass1Lvl", 1);
        Debug.Log("VictoryCanvasEnabled активирован, событие вызвано.");
    }
}
