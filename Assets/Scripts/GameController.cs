using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Variables statics")]
    public static GameController controller;
    public Interface uiController;
    public ListClients listClients;


    [Header("Status jogador:")]
    public float carIntegrityMax; // Integridade do carro
    public float carIntegrityCurrent; // Integridade do carro
    [SerializeField] private float playerFuel = 55; // quantidade atual de gasolina
    public readonly float maxPlayerFuel = 55; // Maximo do tanque de gasolina
    [SerializeField] private const float fuelBurn = 0.001f; // 
    [SerializeField] private float fuelBurnMultiplier = 1f;
    [SerializeField] private short totalClients = 0;

    [SerializeField] public readonly float literPrice = 5.86f;
    private float ratingSum = 0;
    private bool isGamePaused = false;

    public int passwordClient { get; set; }
    [field: SerializeField] public bool passwordCorrect { get; set; }

    public float AvgRating
    {
        get { return (totalClients > 0 ? ratingSum / totalClients : 0); }
    }

    [SerializeField] private float playerMoney = 100f;

    public float PlayerMoney
    {
        get { return playerMoney; }
    }

    //[SerializeField] private Player player;
    //[SerializeField] private Text txtGas;
    //[SerializeField] private Text txtMoney;
    //[SerializeField] private GameObject pauseWidget;

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

    private void Start()
    {
        if (controller == null)
            controller = this;
        else
            Destroy(gameObject);
        listClients = new ListClients();
        DontDestroyOnLoad(this);
        //txtMoney.text = $"{playerMoney:F2}";
    }

    public void FuelCar(float gasoline)
    {
        playerFuel += gasoline;
        playerMoney -= gasoline * literPrice;
        //txtMoney.text = $"{playerMoney:F2}";
        //txtGas.text = playerFuel.ToString();
        uiController.ATTUI();
    }

    public void BurnFuel(float gasInput)
    {
        if (!isGamePaused)
        {
            //txtGas.text = $"{playerFuel:F2}";
            playerFuel -= fuelBurn * fuelBurnMultiplier;
            uiController.ATTUI();
            uiController.Gasolina(playerFuel/maxPlayerFuel);
        }
        if (playerFuel <= 0)
            uiController.GameOver(1);
    }

    public void NewRating(int rating)
    {
        ratingSum += rating / 2;
        totalClients++;
        uiController.ATTUI();
    }

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

    public void GetPaid(float pay)
    {
        playerMoney += pay;
        //txtMoney.text = $"{playerMoney:F2}";
        uiController.ATTUI();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            if (Time.timeScale == 0)
            {
                uiController.pauseUI.SetActive(false);
                Time.timeScale = 1;
            }
            else
            {
                uiController.pauseUI.SetActive(true);
                Time.timeScale = 0;
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
        if (carIntegrityCurrent <= 0)
            uiController.GameOver(0);
        uiController.DamageAnimation();
    }

}
