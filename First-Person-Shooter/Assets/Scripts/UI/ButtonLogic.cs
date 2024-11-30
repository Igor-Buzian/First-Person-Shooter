using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public void PlayButton()
    {
        if (PlayerPrefs.HasKey("Pass1Lvl"))
        {
            SceneManager.LoadScene("lvl 2");
        }
        else
        {
            SceneManager.LoadScene("lvl 1");
        }
        

    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void NextLevel()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("lvl 2");
    }
    public void BackToMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Start Scene");
    }
}
