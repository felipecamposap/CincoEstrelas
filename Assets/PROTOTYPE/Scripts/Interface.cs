using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private Slider carIntegritySlider;
    [SerializeField] private Text gasText;
    [SerializeField] private Text money;
    public GameObject pauseUI;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private Text gameOverText;


    private void Start()
    {
        GameController.controller.uiController = this;
        ATTUI();
    }

    public void ATTUI()
    {
        carIntegritySlider.maxValue = GameController.controller.carIntegrityMax; // Declarando o maximo da integridade do carro possivel no slider
        carIntegritySlider.value = GameController.controller.carIntegrityCurrent; // Declarando o valor atual da integridade do carro possivel no slider
        gasText.text = "Gasolina: " + GameController.controller.PlayerFuel;
        money.text = "R$ " + GameController.controller.PlayerMoney;
        
    }

    public void GameOver(int _value) // 0 - Hp do carro zerado | 1 - Dinheiro zerado
    {
        switch (_value)
        {
            case 0:
                gameOverText.text = "O seu carro está muito danificado!";
                break;

            case 1:
                gameOverText.text = "A gasolina do seu carro acabou!";
                break;

        }
        gameOverObj.SetActive(true);

    }

}
