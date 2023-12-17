using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interface : MonoBehaviour
{
    private Animator estrelasCorridaAnimator;
    [SerializeField] private Slider estrelasCorrida;
    [SerializeField] private Slider carIntegritySlider;
    [SerializeField] private Slider clientRatingPlayerSlider;
    [SerializeField] private Slider playerCurrentRating;
    [SerializeField] private Text money;
    public GameObject pauseUI;
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private Text gameOverText;
    [SerializeField] private Animator celular, app;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private RectTransform panelHistoryClient;
    [SerializeField] private GameObject clientHistoryObj;
    private bool cellphoneLift = false;
    [SerializeField] private RectTransform speedometerPointer;
    [SerializeField] private RectTransform gasPointer;
    public ListClients interfaceListClients = new ListClients(); // Lista de clientes customizada
    private MouseLook camMovement;
    [SerializeField] private GameObject vitoria;
    [SerializeField] private GameObject estrelaCorrida;


    [Header("----- Radio -----")]
    [SerializeField] private Text clipText;
    [SerializeField] private Animator clipAnimator;
    



    private void Start()
    {
        GameController.controller.uiController = this;
        GameController.controller.ResetGC();
        camMovement = Camera.main.transform.parent.gameObject.GetComponent<MouseLook>();
        estrelasCorridaAnimator =  estrelasCorrida.transform.parent.GetComponent<Animator>();
        estrelasCorrida.value = 10;
        ATTUI();
        Invoke("AttList", 2);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            if (cellphoneLift)
            {
                camMovement.LockCam(false);
                CellPhoneAnimation(1);
            }
            else
            {
                camMovement.LockCam(true);
                CellPhoneAnimation(0);
            }
        }
    }

    public void EstrelaCorrida(bool value)
    {
        estrelaCorrida.SetActive(value);
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
        clientRatingPlayerSlider.value = GameController.controller.AvgRating;
        playerCurrentRating.value = GameController.controller.playerStar;
        Gasolina(GameController.controller.PlayerFuel / GameController.controller.maxPlayerFuel);
        estrelasCorrida.value = 10 - (GameController.controller.penalty * 2);

    }

    public void MostrarEstrelaCorrida()
    {
        estrelasCorridaAnimator.Play("MostrarEstrelasCorrida");
    }

    public void EsconderEstrelaCorrida()
    {
        estrelasCorridaAnimator.Play("EsconderEstrelasCorrida");
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

    public void PlayerVitoria()
    {
        vitoria.SetActive(true);
    }

    public void ShowHistoryClients(int sort)
    {
        //Debug.Log(panelHistoryClient.childCount);
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

    public void StartClipAnimation(string _clipName)
    {
        clipText.text = _clipName;
        clipAnimator.SetBool("IsPlaying", true);
    }

    public void StopClipAnimator()
    {
        clipAnimator.SetBool("IsPlaying", false);
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
                celular.Play("SubirCelular");
                break;

            case 1: // abaixar celular
                cellphoneLift = false;
                GameController.controller.ToggleCursor(false);
                celular.Play("DescerCelular");
                break;

            case 2:
                app.Play("CincoEstrelaComecar");
                break;

            case 3: // Mostrar Minimapa
                GameController.controller.ToggleCursor(true);
                app.Play("ComecarCorrida");
                break;

            case 4:
                app.Play("CincoEstrelasComecarTrabalho");
                break;

            case 5: // Mostrar Cliente
                app.Play("CincoEstrelaFechar");
                break;

            case 6: // Password Hud
                GameController.controller.ToggleCursor(true);
                app.Play("Password");
                break;

            case 7: // Corrida concluida
                app.Play("CorridaConcluida");
                break;

            case 8: // Rejeitar corrida
                app.Play("CincoEstrelasRejectClient");
                break;

            case 9: // Fade in
                app.Play("CincoEstrelasHistoryClientsUp");
                break;

            case 10:
                app.Play("CincoEstrelasHistoryBack");
                break;
        }
    }

}
