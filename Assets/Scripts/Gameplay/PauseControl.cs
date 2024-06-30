using UnityEngine;
using UnityEngine.UIElements;


public class PauseControl : MonoBehaviour
{
    bool cursorEnabled = false;
    private void Update()
    {
        if (GameController.controller.isInteracting)
            return;

        if (Input.GetButtonDown("Pause") && GameController.controller.player.inGame)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        Debug.Log("pause");
        if (Time.timeScale == 0)
        {
            GameController.controller.uiController.pauseUI.SetActive(false);
            Time.timeScale = 1;
            GameController.controller.audioSource.reverbZoneMix = 0;
            GameController.controller.audioSource.volume += GameController.controller.audioSource.volume;
            GameController.controller.audioSource.pitch = 1;
            GameController.controller.ToggleCursor(cursorEnabled);
        }
        else
        {
            cursorEnabled = UnityEngine.Cursor.visible;
            GameController.controller.uiController.pauseUI.SetActive(true);
            Time.timeScale = 0;
            GameController.controller.audioSource.reverbZoneMix = 1;
            GameController.controller.audioSource.volume /= 2;
            GameController.controller.audioSource.pitch = 0.99f;
            GameController.controller.ToggleCursor(true);
        }
    }
}