using UnityEngine;


public class GameController : MonoBehaviour
{
    [Header("Variables statics")]
    public static GameController controller;
    public Interface uiController;
    [field: SerializeField] public ListClients listClients;
    public AudioSource audioSource;
    public AlvoMinimapa alvoMinimapa;
    public PlayerMovement player;


    [Header("Status jogador:")]
    public float carIntegrityMax; // Integridade do carro
    public float carIntegrityCurrent; // Integridade do carro
    [SerializeField] private float playerFuel = 55; // quantidade atual de gasolina
    public readonly float maxPlayerFuel = 55; // Maximo do tanque de gasolina
    [SerializeField] private const float fuelBurn = 0.001f; // 
    [SerializeField] private float fuelBurnMultiplier = 1f;
    [SerializeField] private short totalClients = 0;
    public int penalty = 0;
    [SerializeField] private float playerMoney = 100f;
    public int playerStar = 0;

    [SerializeField] public readonly float literPrice = 5.86f;
    private float ratingSum = 0;
    private bool isGamePaused = false;

    public int passwordClient { get; set; }
    public bool passwordCorrect;

    public float AvgRating
    {
        get { return (totalClients > 0 ? ratingSum / totalClients : 0); }
    }

    public float PlayerMoney
    {
        get { return playerMoney; }
    }

    public float PlayerFuel
    {
        get { return playerFuel; }
    }

    public float AvailableFuelSpace
    {
        get { return maxPlayerFuel - playerFuel; }
    }

    public float BuyableLiters
    {
        get { return (playerMoney / literPrice); }
    }

    public Transform[] minimapaAlvo = new Transform[2];

    [Header("Trapa�as: ")]
    public bool[] trapacas = new bool[3];


    private void Awake()
    {
        if (controller == null)
            controller = this;
        else
            Destroy(gameObject);
        listClients = new ListClients();
        DontDestroyOnLoad(this);
    }

    public void ToggleCursor(bool value)
    {
        if(value)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
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
        uiController.ATTUI();

    }

    public void FuelCar(float gasoline)
    {
        playerFuel += gasoline;
        playerMoney -= gasoline * literPrice;
        uiController.ATTUI();
    }

    public void ResetGC()
    {
        listClients = new ListClients();
        carIntegrityCurrent = carIntegrityMax;
        playerMoney = 100f;
        playerFuel = maxPlayerFuel;
        totalClients = 0;
        if (trapacas[2])
            trapacas[2] = false;
        else
            playerStar = 0;
        uiController.ATTUI();
        if (playerStar >= 10)
            PlayerVitoria();

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
            uiController.Gasolina(playerFuel/maxPlayerFuel);
        }
        if (playerFuel <= 0)
            uiController.GameOver(1);
    }

    public void NewRating()
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

    private bool[] cursorState = new bool[2];
    public void SetGamePaused(bool value)
    {
        if (value == true)
        {
            isGamePaused = true;
            Time.timeScale = 0;

        }
        else
        {
            isGamePaused = false;
            Time.timeScale = 1;
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

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && player.inGame)
        {
            if (Time.timeScale == 0)
            {
                uiController.pauseUI.SetActive(false);
                Time.timeScale = 1;
                audioSource.reverbZoneMix = 0;
                audioSource.volume += audioSource.volume;
                audioSource.pitch = 1;


                Cursor.visible = cursorState[0];
                if (cursorState[1])
                    Cursor.lockState = CursorLockMode.Locked;
                else
                    Cursor.lockState = CursorLockMode.None;
                
            }
            else
            {
                uiController.pauseUI.SetActive(true);
                Time.timeScale = 0;
                audioSource.reverbZoneMix = 1;
                audioSource.volume /= 2;
                audioSource.pitch = 0.99f;

                if (Cursor.visible)
                    cursorState[0] = true;
                else
                    cursorState[0] = false;
                if (Cursor.lockState == CursorLockMode.Locked)
                    cursorState[1] = true;
                else
                    cursorState[1] = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

            }

        }

    }

    public void PasswordClient()
    {
        passwordClient = Random.Range(1000, 9999);
    }

    public void Damage(float _value)
    {
        carIntegrityCurrent -= _value;
        if (carIntegrityCurrent <= 0){
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
