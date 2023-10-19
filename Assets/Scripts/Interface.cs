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
    [SerializeField] private Animator cellPhoneAnimator;


    private void Start()
    {
        GameController.controller.uiController = this;
        ATTUI();
    }

    public void ATTUI()
    {
        carIntegritySlider.maxValue = GameController.controller.carIntegrityMax; // Declarando o maximo da integridade do carro possivel no slider
        carIntegritySlider.value = GameController.controller.carIntegrityCurrent; // Declarando o valor atual da integridade do carro possivel no slider
        gasText.text = $"Gasolina: {GameController.controller.PlayerFuel:F2}L";
        money.text = $"R${GameController.controller.PlayerMoney:F2}";
        
    }

    public void GameOver(int _value) // 0 - Hp do carro zerado | 1 - Dinheiro zerado
    {
        switch (_value)
        {
            case 0:
                gameOverText.text = "O seu carro est� muito danificado!";
                break;

            case 1:
                gameOverText.text = "A gasolina do seu carro acabou!";
                break;

        }
        gameOverObj.SetActive(true);

    }


    //Animações:
    // 0 - Levantar celular
    // 1 - CincoEstrela abrir aplicativo home
    // 2 - CincoEstrela Menu inicial começar
    // 3 - Começar corrida
    // 4 - Cliente encontrado
    // 5 - Aceitar corrida
    // 6 - Rejeitar corrida
    // ?? - Sair Aplicativo Cinco Estrelas
    public void CellPhoneAnimation(int _value)// 
    {
        switch (_value)
        {
            case 0:
                cellPhoneAnimator.Play("LiftCellPhone");
                break;

            case 1:
                cellPhoneAnimator.Play("CincoEstrelaTransition");
                break;

            case 2:
                cellPhoneAnimator.Play("CincoEstrelaHomeStart");
                break;

            case 3:
                cellPhoneAnimator.Play("CincoEstrelasStartJob");
                break;

            case 4:
                cellPhoneAnimator.Play("CincoEstrelasClientStart");
                break;

            case 5:
                //cellPhoneAnimator.Play("CincoEstrelasClientStart");
                break;

            case 6:
                cellPhoneAnimator.Play("CincoEstrelasRejectClient");
                break;

            case 7:
                cellPhoneAnimator.Play("CincoEstrelasLowerCellphoneAceptJob");
                break;

            case 8:
                cellPhoneAnimator.Play("CincoEstrelasLiftCellPhoneAceptJob");
                break;
        }
    }

}
