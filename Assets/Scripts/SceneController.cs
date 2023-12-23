using System;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneController : MonoBehaviour
{

    public void LoadScene(int index)
    {
        if(index == 0){
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GameController.controller.trapacas[2] = false;
            GameController.controller.playerStar = 0;
        }
        Time.timeScale = 1.0f;
        SceneManager.LoadScene(index);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
