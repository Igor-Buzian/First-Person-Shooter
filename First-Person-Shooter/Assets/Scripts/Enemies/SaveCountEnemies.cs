using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves the count of child objects (enemies) to PlayerPrefs.
/// </summary>
public class SaveCountEnemies : MonoBehaviour
{
    /// <summary>
    /// Called on the frame when the script is enabled.
    /// Retrieves the number of child objects and saves it to PlayerPrefs.
    /// </summary>
    void Start()
    {
        // Get the count of child objects
        int count = transform.childCount;

        // Save the count to PlayerPrefs
        PlayerPrefs.SetInt("EnemyCount", count);
        PlayerPrefs.Save(); // Save changes to PlayerPrefs
    }
}