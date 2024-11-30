using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonLogic : MonoBehaviour
{
    public void PlayButton()
    {
        SceneManager.LoadScene("lvl 2");
    }
    public void QuitButton()
    {
        Application.Quit();
    }
    public void RespawnButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("lvl 2");
    }
    public void BackToMenuButton()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("lvl 2");
    }
}
