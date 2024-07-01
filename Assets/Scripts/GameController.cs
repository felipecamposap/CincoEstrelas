using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using RenderSettings = UnityEngine.RenderSettings;

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
    public float maxPlayerFuel = 55; // Maximo do tanque de gasolina
    [SerializeField] private const float fuelBurn = 0.001f; // 
    [SerializeField] private float fuelBurnMultiplier = 1f;
    [SerializeField] private short totalClients = 0;
    public int penalty = 0;
    [SerializeField] private float playerMoney = 100f;
    public int playerStar = 0;

    [SerializeField] public const float literPrice = 5.86f;
    private float ratingSum = 0;
    private bool isGamePaused = true;
    public bool isInteracting = false;

    public int passwordClient { get; set; }
    [HideInInspector] public bool passwordCorrect;

    // ----- Config Upgrades
    [Header("Upgrades Config")] [SerializeField]
    public const float upgradePrice = 100f;


    // ----- Tempo Jogo
    [Header("Tempo Jogo")] [SerializeField]
    private int hour, minute;

    [SerializeField] private GameObject dayNightCycle;
    private Light sun, moon;
    [SerializeField] private Material skybox, predioBloom, neonDetail;

    [SerializeField] private Color dayColorHorizon,
        nightColorHorizon,
        nightFogColor,
        dayFogColor,
        nightPredioBloom,
        dayPredioBloom,
        nightNeonDetail,
        dayNeonDetail;

    private float sunIntensity, moonIntensity, lightTimer;
    private float timerMinute, dayNightTimer;

    private float nightSizeDarknessUp = 2f,
        daySizeDarknessUp = 1.5f;

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

    [HideInInspector] public int[] soundValues = new int[3] ;
    [HideInInspector] public bool startSound = true;


    private void Awake()
    {
        listClients = new ListClients();
        DontDestroyOnLoad(this);

        if (!controller)
            controller = this;
        else
            Destroy(gameObject);

        ToggleCursor(SceneManager.GetActiveScene().name == "Menu");
        nightNeonDetail *= Mathf.Pow(2, 6.5f);
        dayNeonDetail *= Mathf.Pow(2, 3f);
        nightPredioBloom *= Mathf.Pow(2, 3f);
        //dayPredioBloom *= 1f;
        ResetColors();
    }
    
    

    public void Update()
    {
        if (isGamePaused) return;
        if (!(timerMinute < Time.time)) return;
        minute++;
        if (minute >= 60)
        {
            minute = 0;
            hour++;
        }

        timerMinute = Time.time + 1.25f;
        if (hour == 6)
        {
            isGamePaused = true;
            player.inGame = false;
            if (playerMoney - GetDailyBill() <= 0)
            {
                uiController.GameOver(1);
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                if (alvoMinimapa)
                {
                    ResetClient();
                    GetPaid(0, false);
                }
            }
            else
                uiController.NextDay();
        }

        uiController.SetHour(hour, minute);
    }

    public void ResetClient()
    {
        if (minimapaAlvo.IsUnityNull()) return;
        foreach (var t in minimapaAlvo)
            Destroy(t.gameObject);
    }

    private void FixedUpdate()
    {
        if (SceneManager.GetActiveScene().name == "Luminopolis")
            UpdateDayNightCycle();
    }

    private void UpdateDayNightCycle()
    {
        if (!dayNightCycle)
        {
            dayNightCycle = GameObject.FindWithTag("DayNightCycle");
        }

        if (!sun || !moon)
        {
            sun = dayNightCycle.transform.GetChild(0).GetChild(0).GetComponent<Light>();
            sunIntensity = sun.intensity;
            moon = dayNightCycle.transform.GetChild(1).GetChild(0).GetComponent<Light>();
            moonIntensity = moon.intensity;
        }

        if (hour < 6)
        {
            // Calculate the target angle based on the current time
            dayNightTimer += ((Time.fixedDeltaTime * 0.75f) / 360f); // Fractional hours
        }
        else
        {
            dayNightTimer = 0f;
        }
        
        var targetAngle = Mathf.Lerp(0, 90, dayNightTimer);

        if (hour >= 4 && minute >= 50)
        {
            sun.gameObject.SetActive(true);
            lightTimer += Time.fixedDeltaTime / 6;
            sun.intensity = Mathf.Lerp(0f, sunIntensity, lightTimer);
            moon.intensity = Mathf.Lerp(moonIntensity, 0f, lightTimer);
            if (moon.intensity == 0)
            {
                moon.gameObject.SetActive(false);
            }

            skybox.SetColor(ColorHorizon, Color.Lerp(nightColorHorizon, dayColorHorizon, lightTimer));
            skybox.SetFloat(SizeDarknessUp, Mathf.Lerp(nightSizeDarknessUp, daySizeDarknessUp, lightTimer));
            RenderSettings.fogColor = Color.Lerp(nightFogColor, dayFogColor, lightTimer / 3);
            predioBloom.SetColor(EmissionColor, Color.Lerp(nightPredioBloom, dayPredioBloom, lightTimer));
            neonDetail.SetColor(EmissionColor, Color.Lerp(nightNeonDetail, dayNeonDetail, lightTimer));
        }

        // Apply the rotation
        var currentRotation = dayNightCycle.transform.eulerAngles;
        currentRotation.x = targetAngle;
        dayNightCycle.transform.eulerAngles = currentRotation;
    }

    private void ResetColors()
    {
        skybox.SetColor(ColorHorizon, nightColorHorizon);
        skybox.SetFloat(SizeDarknessUp, nightSizeDarknessUp);
        predioBloom.SetColor(EmissionColor, nightPredioBloom);
        neonDetail.SetColor(EmissionColor, nightNeonDetail);
        RenderSettings.fogColor = nightFogColor;
    }

    private void ResetDayNightCycle()
    {
        ResetColors();
        sun.intensity = sunIntensity;
        moon.intensity = moonIntensity;
        sun.gameObject.SetActive(false);
        moon.gameObject.SetActive(true);
        lightTimer = 0f;
        dayNightTimer = 0f;

        var currentRotation = dayNightCycle.transform.eulerAngles;
        currentRotation.x = 0f;
        dayNightCycle.transform.eulerAngles = currentRotation;

        sun.intensity = sunIntensity;
        moon.intensity = moonIntensity;
        sun.gameObject.SetActive(false);
        moon.gameObject.SetActive(true);
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
        //GetPaid(_value, false);
        uiController?.ATTUI(); // Checar UI
    }

    public void FuelCar(float gasoline)
    {
        playerFuel += gasoline;
        playerMoney -= gasoline * literPrice;
        uiController.ATTUI();
    }

    public void ResetGC()
    {
        isInteracting = false;
        hour = 0;
        minute = 0;
        listClients = new ListClients();
        carIntegrityCurrent = carIntegrityMax;
        playerMoney = 10f;
        maxPlayerFuel = 55f;
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
        ResetDayNightCycle();
    }

    public float GetDailyBill()
    {
        var debtAll = (vwater + vlight + internet + netMobile + iptu + food + debtDay) / 30;
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
        ResetDayNightCycle();
        ResetClient();
        GetPaid(0, false);
    }

    public void PlayerVitoria()
    {
        uiController.PlayerVitoria();
        player.PlayerVictory();
        isGamePaused = true;
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

    // --------- Sistema tempo cliente --------- \\
    int timeClient;
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    private static readonly int SizeDarknessUp = Shader.PropertyToID("_SizeDarknessUp");
    private static readonly int ColorHorizon = Shader.PropertyToID("_ColorHorizon");

    public void StartClientTime()
    {
        timeClient = Mathf.Max(35,
            (int)(Vector3.Distance(player.transform.position, minimapaAlvo[0].position) * 0.075f));
        StartCoroutine("ClientTime");
    }

    private IEnumerator ClientTime()
    {
        uiController.AttClientTime(timeClient);
        yield return new WaitForSeconds(1);
        timeClient--;
        if (timeClient > 0)
            StartCoroutine("ClientTime");
        else
        {
            minimapaAlvo[0].GetComponent<Client>().LoseClient(2);
            yield return new WaitForSeconds(2.2f);
            ResetClient();
            GetPaid(0, false);
        }
    }

    public void StopClientTime()
    {
        StopCoroutine("ClientTime");
    }
    // --------- Fim Sistema tempo cliente --------- \\

    private int NewRating()
    {
        var rating = Mathf.Clamp(10 - (penalty * 2), 2, 10);
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
        return rating;
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

    public int GetPaid(float pay, bool isJob)
    {
        playerMoney += pay;
        if (!isJob)
        {
            uiController.CellPhoneAnimation(2);
            return 0;
        }

        uiController.CellPhoneAnimation(7);
        int rating = NewRating();
        uiController.EsconderEstrelaCorrida();
        uiController.ATTUI();
        return rating;
    }

    public void ChangeMoney(float value)
    {
        playerMoney += value;
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