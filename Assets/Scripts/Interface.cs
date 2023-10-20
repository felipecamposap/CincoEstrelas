using System;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    [SerializeField] private Slider carIntegritySlider;
    [SerializeField] private Slider playerRatingSlider;
    [SerializeField] private Text gasText;
    [SerializeField] private Text money;
    public GameObject pauseUI;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Animator cellPhoneAnimator;
    public bool working { get; set; }

    [SerializeField] private RectTransform panelHistoryClient;
    [SerializeField] private RectTransform clientBeforeTransform;
    [SerializeField] private GameObject clientHistoryObj;

    private void Start()
    {

        GameController.controller.uiController = this;
        //GameController.controller.ToggleCursor(false);
        ATTUI();
    }

    public void ATTUI()
    {
        carIntegritySlider.maxValue = GameController.controller.carIntegrityMax; // Declarando o maximo da integridade do carro possivel no slider
        carIntegritySlider.value = GameController.controller.carIntegrityCurrent; // Declarando o valor atual da integridade do carro possivel no slider
        gasText.text = $"Gasolina: {GameController.controller.PlayerFuel:F2}L";
        money.text = $"R${GameController.controller.PlayerMoney:F2}";
        playerRatingSlider.value = GameController.controller.AvgRating;
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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && !working)
        {
            CellPhoneAnimation(0);
        }
    }

    public void ShowHistoryClients()
    {
        panelHistoryClient.offsetMin = new Vector2(4.6f, MathF.Max(GameController.controller.listClients.totalClients - 4, 1) * 80);
        for (int i = 1; i <= GameController.controller.listClients.totalClients; i++)
        {
            GameObject clone = Instantiate(clientHistoryObj, clientBeforeTransform.position, Quaternion.identity, panelHistoryClient);
            ClientsParameters client = new ClientsParameters("", 0, 0f);
            GameController.controller.listClients.GetClient(i, client);
            clone.GetComponent<ClientsHud>().GetClientsInfo(client);
        }

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
                GameController.controller.ToggleCursor(true);
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
                GameController.controller.ToggleCursor(false);
                cellPhoneAnimator.Play("CincoEstrelasLowerCellphoneAceptJob");
                break;

            case 8:
                GameController.controller.ToggleCursor(true);
                cellPhoneAnimator.Play("CincoEstrelasLiftCellPhoneAceptJob");
                break;
        }
    }

}
