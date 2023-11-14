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
    [SerializeField] private Animator damageAnimator;
    public bool working { get; set; }
    [SerializeField] private RectTransform panelHistoryClient;
    [SerializeField] private Transform clientLogSpawn;
    [SerializeField] private GameObject clientHistoryObj;
    private float clientLogHeight = 75;
    private int clientListLength = 1;

    private bool cellphoneLift = false;
    [SerializeField] private RectTransform speedometerPointer;
    [SerializeField] private RectTransform gasPointer;



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
        money.text = $"R${GameController.controller.PlayerMoney:F2}";
        playerRatingSlider.value = GameController.controller.AvgRating;
        Gasolina(GameController.controller.PlayerFuel / GameController.controller.maxPlayerFuel);
    }

    public void Velocity(float _value)
    {
        //Debug.Log(_value + " - " + Vector3.Lerp(-251, -20, Math.Abs(_value));
        speedometerPointer.eulerAngles = new Vector3(0, 0, Mathf.Lerp(4, -190, Math.Abs(_value)));
    }

    public void Gasolina(float _value)
    {
        gasPointer.eulerAngles = new Vector3(0, 0, Mathf.Lerp(28, -58, Math.Abs(_value)));
    }

    public void DamageAnimation()
    {
        damageAnimator.Play("Damage");
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
            if (cellphoneLift)
                CellPhoneAnimation(1);
            else
                CellPhoneAnimation(0);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            ShowHistoryClients();
        }
    }

    public void ShowHistoryClients()
    {

        if (GameController.controller.listClients.totalClients > clientListLength /*- 1*/)
        {
            for (int i = clientListLength; i <= GameController.controller.listClients.totalClients; i++)
            {
                ClientsParameters client = new ClientsParameters("", 0, 0f);
                GameController.controller.listClients.GetClient(i, out client);
                GameObject clone = Instantiate(clientHistoryObj, clientLogSpawn.position, Quaternion.identity, panelHistoryClient);
                clone.GetComponent<ClientsHud>().GetClientsInfo(client);
                clientLogSpawn.position = new Vector3(clientLogSpawn.position.x, clientLogSpawn.position.y - clientLogHeight, clientLogSpawn.position.z);
                clientListLength++;
                panelHistoryClient.GetComponent<RectTransform>( ).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, clientLogHeight * (clientListLength /*- 1*/));
            }
        }
        CellPhoneAnimation(9);

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
            case 0: // Levantar celular
                GameController.controller.ToggleCursor(true);
                cellphoneLift = true;
                cellPhoneAnimator.Play("LiftCellPhone");
                break;

            case 1: // abaixar celular
                cellphoneLift = false;
                GameController.controller.ToggleCursor(false);
                cellPhoneAnimator.Play("LowerCellPhone");
                break;

            case 2:
                cellPhoneAnimator.Play("CincoEstrelaTransition");
                break;

            case 3:
                GameController.controller.ToggleCursor(true);
                cellPhoneAnimator.Play("CincoEstrelaHomeStart");
                break;

            case 4:
                cellPhoneAnimator.Play("CincoEstrelasStartJob");
                break;

            case 5: // Mostrar Cliente
                cellPhoneAnimator.Play("CincoEstrelasClientStart");
                break;

            case 6: // Password Hud
                GameController.controller.ToggleCursor(true);
                cellPhoneAnimator.Play("Password");
                break;

            case 7: // Corrida concluida
                cellPhoneAnimator.Play("JobCompleted");
                break;

            case 8: // Rejeitar corrida
                cellPhoneAnimator.Play("CincoEstrelasRejectClient");
                break;

            case 9: // Fade in
                cellPhoneAnimator.Play("CincoEstrelasHistoryClientsUp");
                break;

            case 10:
                cellPhoneAnimator.Play("CincoEstrelasHistoryBack");
                break;
        }
    }

}
