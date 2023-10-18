using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private Slider carIntegritySlider;
    [SerializeField] private Text gasText;
    [SerializeField] private Text money;
    public GameObject pauseUI;
    [SerializeField] private Text gameOverText;
    [SerializeField] private GameObject gameOver;

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

    public void GameOver(int index)
    {
        switch (index) // 0 - Carro estragado | 1 - Gasolina acabou
        {
            case 0:
                gameOverText.text = "Seu carro está muito danificado!";
                break;

            case 1:
                gameOverText.text = "A sua gasolina esgotou!";
                break;
        }
        gameOver.SetActive(true);
    }

}
