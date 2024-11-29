using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveCountEnemies : MonoBehaviour
{
    void Start()
    {
        // �������� ���������� �������� ��������
        int count = transform.childCount;

        // ��������� ���������� � PlayerPrefs
        PlayerPrefs.SetInt("EnemyCount", count);
        PlayerPrefs.Save(); // ��������� ���������
    }
}