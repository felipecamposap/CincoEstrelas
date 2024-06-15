using UnityEngine;
using UnityEngine.SceneManagement;


[DefaultExecutionOrder(-99)]
public class GameController : MonoBehaviour
{
    [Header("Variables statics")] public static GameController controller;
    public Interface uiController;
    [field: SerializeField] public ListClients listClients;
    public AudioSource audioSource;
    public AlvoMinimapa alvoMinimapa;
    public PlayerMovement player;
    public ObserverTrafficLight obsTrafficLight;


    [Header("Status jogador:")] public float carIntegrityMax; // Integridade do carro
    public float carIntegrityCurrent; // Integridade do carro
    [SerializeField] private float playerFuel = 55; // quantidade atual de gasolina
    public readonly float maxPlayerFuel = 55; // Maximo do tanque de gasolina
    [SerializeField] private const float fuelBurn = 0.001f; // 
    [SerializeField] private float fuelBurnMultiplier = 1f;
    [SerializeField] private short totalClients = 0;
    public int penalty = 0;
    [SerializeField] private float playerMoney = 100f;
    public int playerStar = 0;
    //public float lastMoney;

    [SerializeField] public const float literPrice = 5.86f;
    private float ratingSum = 0;
    private bool isGamePaused = true;

    public int passwordClient { get; set; }
    [HideInInspector] public bool passwordCorrect;

    // ----- Config Upgrades
    [Header("Upgrades Config")] [SerializeField]
    public readonly float motorUpgradePrice = 100f;


    // ----- Tempo Jogo
    [Header("Tempo Jogo")] [SerializeField]
    private int hour, minute;

    private float timerMinute;

    [Header("Gastos")] public readonly float vwater = 80;
    public readonly float vlight = 300;
    public readonly float internet = 100;
    public readonly float netMobile = 50;
    public readonly float iptu = 125;
    public readonly float food = 600;
    public float debtDay = 0;

    public float AvgRating => (totalClients > 0 ? ratingSum / totalClients : 0);

    public float PlayerMoney => playerMoney;

    public float PlayerFuel => playerFuel;

    public float AvailableFuelSpace => maxPlayerFuel - playerFuel;

    public float BuyableLiters => (playerMoney / literPrice);

    public Transform[] minimapaAlvo = new Transform[2];

    [Header("Trapa�as: ")] public bool[] trapacas = new bool[3];


    private void Awake()
    {
        listClients = new ListClients();
        DontDestroyOnLoad(this);

        if (controller == null)
            controller = this;
        else
            Destroy(gameObject);

        ToggleCursor(SceneManager.GetActiveScene().name == "Menu");
    }

    public void Update()
    {
        if (isGamePaused) return;
        if (!(timerMinute < Time.time)) return;
        minute++;
        if (minute == 60)
        {
            minute = 0;
            hour++;
        }

        timerMinute = Time.time + 1.25f;
        if (hour == 6)
        {
            isGamePaused = true;
            player.inGame = false;
            //Debug.Log(playerMoney - GetDailyBill());
            if (playerMoney - GetDailyBill() <= 0)
            {
                uiController.GameOver(1);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (alvoMinimapa != null)
                {
                    foreach (Transform item in minimapaAlvo)
                        Destroy(item);
                    GetPaid(0,true);
                }

            }
            else
                uiController.NextDay();
        }

        uiController.SetHour(hour, minute);

    }

    public void ToggleCursor(bool value)
    {
        if (value)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void RecoverIntegrity(float _integrity, float _value)
    {
        carIntegrityCurrent += _integrity;
        GetPaid(_value, false);
        uiController?.ATTUI(); // Checar UI
    }

    public void FuelCar(float gasoline)
    {
        playerFuel += gasoline;
        GetPaid(gasoline * literPrice, false);
        uiController.ATTUI();
    }

    public void ResetGC()
    {
        hour = 0;
        minute = 0;
        listClients = new ListClients();
        carIntegrityCurrent = carIntegrityMax;
        playerMoney = 10f;
        playerFuel = maxPlayerFuel;
        totalClients = 0;
        if (trapacas[1])
            trapacas[1] = false;
        else
            playerStar = 0;
        uiController.ATTUI();
        if (playerStar >= 10)
            PlayerVitoria();
        isGamePaused = false;
    }

    public float GetDailyBill()
    {
        float debtAll = (vwater + vlight + internet + netMobile + iptu + food + debtDay) / 30;
        return debtAll;
    }

    public void NextDay()
    {
        hour = 0;
        minute = 0;
        if (playerMoney < GetDailyBill())
            GetPaid(debtDay = playerMoney - GetDailyBill(), false);
        else
            debtDay = 0;
        isGamePaused = false;
        player.inGame = true;
    }

    public void PlayerVitoria()
    {
        uiController.PlayerVitoria();
        player.PlayerVictory();
    }

    public void BurnFuel(float gasInput)
    {
        if (!isGamePaused)
        {
            playerFuel -= fuelBurn * fuelBurnMultiplier;
            uiController.ATTUI();
            uiController.Gasolina(playerFuel / maxPlayerFuel);
        }

        if (playerFuel <= 0)
            uiController.GameOver(1);
    }

    private void NewRating()
    {
        int rating = Mathf.Clamp(10 - (penalty * 2), 2, 10);
        ratingSum += rating;
        totalClients++;
        if (rating > playerStar)
            playerStar++;
        else if (rating < playerStar)
            playerStar--;
        uiController.ATTUI();
        if (playerStar >= 10)
            PlayerVitoria();
        penalty = 0;
    }

    private void SetGamePaused(bool value)
    {
        if (value == true)
        {
            isGamePaused = true;
            Time.timeScale = 0;
            ToggleCursor(true);
        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1;
            ToggleCursor(false);
        }
    }

    public void GetPaid(float pay, bool isJob)
    {
        playerMoney += pay;
        if (!isJob)
            return;
        NewRating();
        uiController.CellPhoneAnimation(7);
        uiController.EsconderEstrelaCorrida();
        uiController.ATTUI();
    }

    public void AddHistoryClient(ClientsParameters client)
    {
        listClients.Insert(client);
    }

    public void PasswordClient()
    {
        passwordClient = Random.Range(1000, 9999);
    }

    public void Damage(float _value)
    {
        carIntegrityCurrent -= _value;
        if (carIntegrityCurrent <= 0)
        {
            uiController.GameOver(0);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        uiController.DamageAnimation();
        uiController.ATTUI();
    }

    public void Interaction(bool value)
    {
        ToggleCursor(value);
        SetGamePaused(value);
    }
}