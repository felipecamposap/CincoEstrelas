using System;
using System.Collections.Generic;
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
    [SerializeField] private RectTransform panelHistoryClient;
    [SerializeField] private GameObject clientHistoryObj;
    private bool cellphoneLift = false;
    [SerializeField] private RectTransform speedometerPointer;
    [SerializeField] private RectTransform gasPointer;
    public ListClients interfaceListClients = new ListClients(); // Lista de clientes customizada



    private void Start()
    {
        ATTUI();
        Invoke("AttList", 2);
    }

    public void AttList()
    {
        /*ClientsParameters teste = new ClientsParameters("Felipe", 5, 5f);
        ClientsParameters teste2 = new ClientsParameters("Gabriel", 3, 5f);
        ClientsParameters teste3 = new ClientsParameters("Igor", 1, 5f);
        ClientsParameters teste4 = new ClientsParameters("Gustavo", 4, 5f);
        ClientsParameters teste5 = new ClientsParameters("Samuel", 2, 5f);
        ClientsParameters teste6 = new ClientsParameters("Arthur", 3, 5f);
        ClientsParameters teste7 = new ClientsParameters("Henrique", 1, 5f);
        GameController.controller.listClients.Insert(teste);
        GameController.controller.listClients.Insert(teste2);
        GameController.controller.listClients.Insert(teste3);
        GameController.controller.listClients.Insert(teste4);
        GameController.controller.listClients.Insert(teste5);
        GameController.controller.listClients.Insert(teste6);
        GameController.controller.listClients.Insert(teste7);*/
        GameController.controller.uiController = this;
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
                gameOverText.text = "O seu carro está muito danificado!";
                break;

            case 1:
                gameOverText.text = "A gasolina do seu carro acabou!";
                break;

        }
        gameOverObj.SetActive(true);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (cellphoneLift)
                CellPhoneAnimation(1);
            else
                CellPhoneAnimation(0);
        }
    }

    public void ShowHistoryClients(int sort)
    {
        Debug.Log(panelHistoryClient.childCount);
        if (panelHistoryClient.childCount > 0)
        {
            ResetHistoryClients();
        }
        interfaceListClients = new ListClients();
        for (int i = 0; i < GameController.controller.listClients.totalClients; i++)
        {
            ClientsParameters client;
            GameController.controller.listClients.GetClient(i, out client);
            interfaceListClients.Insert(client, sort);
        }
        FillHistoryClients();
        CellPhoneAnimation(9);

    }

    public void FillHistoryClients()
    {
        Debug.Log("Fill");
        for (int i = 0; i < interfaceListClients.totalClients; i++)
        {
            ClientsParameters client; //ponteiro para receber valores de clientes
            interfaceListClients.GetClient(i, out client); //recebe valores da lista de clientes
            GameObject clone = Instantiate(clientHistoryObj, panelHistoryClient); //salva e instancia gameobject no lugar correto
            clone.GetComponent<ClientsHud>().GetClientsInfo(client); //transfere informações do cliente para a instancia
        }
    }

    public void ResetHistoryClients()
    {
        
        for (int i = panelHistoryClient.childCount - 1; i >= 0; i--)
        {
            Destroy(panelHistoryClient.GetChild(i).gameObject);
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
