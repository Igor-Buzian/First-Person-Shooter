using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCountEnemies : MonoBehaviour
{
    void Start()
    {
        // Получаем количество дочерних объектов
        int count = transform.childCount;

        // Сохраняем количество в PlayerPrefs
        PlayerPrefs.SetInt("EnemyCount", count);
        PlayerPrefs.Save(); // Сохраняем изменения
    }
}