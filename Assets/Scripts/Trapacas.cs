using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Trapacas : MonoBehaviour
{
    [SerializeField] Toggle[] toggles;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        toggles[0].isOn = GameController.controller.trapacas[0];
        toggles[1].isOn = GameController.controller.trapacas[1];
    }

    public void Indestrutivel(bool _value)
    {
        GameController.controller.trapacas[0] = _value;
    }

    public void GasolinaInfinita(bool _value)
    {
        GameController.controller.trapacas[1] = _value;
    }

    public void Estrelas(string _value)
    {
        int _newValue = int.Parse(_value);
        if (_newValue >= 0 && _newValue < 6){
            GameController.controller.playerStar = _newValue * 2;
            GameController.controller.trapacas[2] = true;
            GameController.controller.uiController.ATTUI();
            if (GameController.controller.playerStar == 10 && SceneManager.GetActiveScene().buildIndex == 1){
                GameController.controller.PlayerVitoria();
                GameController.controller.uiController.pauseUI.SetActive(false);
                Time.timeScale = 1;
                GameController.controller.audioSource.reverbZoneMix = 0;
                GameController.controller.audioSource.volume += GameController.controller.audioSource.volume;
                GameController.controller.audioSource.pitch = 1;
            }

        }

    }

}
