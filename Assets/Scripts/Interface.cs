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
    [SerializeField] private Transform clientLogSpawn;
    [SerializeField] private GameObject clientHistoryObj;
    private float clientLogHeight = 75;

    private void Start()
    {
        ClientsParameters teste = new ClientsParameters("Felipe", 2, 5f);
        ClientsParameters teste2 = new ClientsParameters("Gabriel", 2, 5f);
        ClientsParameters teste3 = new ClientsParameters("Igor", 2, 5f);
        ClientsParameters teste4 = new ClientsParameters("Gustavo", 2, 5f);
        ClientsParameters teste5 = new ClientsParameters("Samuel", 2, 5f);
        ClientsParameters teste6 = new ClientsParameters("Arthur", 2, 5f);
        ClientsParameters teste7 = new ClientsParameters("Henrique", 2, 5f);
        GameController.controller.listClients.Insert(teste);
        GameController.controller.listClients.Insert(teste2);
        GameController.controller.listClients.Insert(teste3);
        GameController.controller.listClients.Insert(teste4);
        GameController.controller.listClients.Insert(teste5);
        GameController.controller.listClients.Insert(teste6);
        GameController.controller.listClients.Insert(teste7);
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowHistoryClients();
        }
    }

    public void ShowHistoryClients()
    {
        Debug.Log(GameController.controller.listClients.totalClients);
        if (GameController.controller.listClients.totalClients > 6)
        {
            panelHistoryClient.offsetMin = new Vector2(0f, panelHistoryClient.offsetMin.y - (75 * (GameController.controller.listClients.totalClients - 6)));
        }

        for (int i = 1; i <= GameController.controller.listClients.totalClients; i++)
        {
            ClientsParameters client = new ClientsParameters("", 0, 0f);
            GameController.controller.listClients.GetClient(i, out client);
            GameObject clone = Instantiate(clientHistoryObj, clientLogSpawn.position, Quaternion.identity, panelHistoryClient);
            clone.GetComponent<ClientsHud>().GetClientsInfo(client);
            clientLogSpawn.position = new Vector3(clientLogSpawn.position.x, clientLogSpawn.position.y - clientLogHeight, clientLogSpawn.position.z);
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
