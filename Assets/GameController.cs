using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool isGamePaused = false;

    [SerializeField] private float playerFuel = 55;
    public readonly float maxPlayerFuel = 55;
    [SerializeField] private const float fuelBurn = 0.001f;
    [SerializeField] private float fuelBurnMultiplier = 1f;
    [SerializeField] public readonly float literPrice = 5.86f;

    private float ratingSum = 0;
    private short totalClients = 0;

    public float AvgRating
    {
        get { return (totalClients > 0 ? ratingSum / totalClients : 0); }
    }

    [SerializeField] private float playerMoney = 100f;

    public float PlayerMoney
    {
        get { return playerMoney; }
    }

    [SerializeField] private Player player;
    [SerializeField] private Text txtGas;
    [SerializeField] private Text txtMoney;

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

    private void Start()
    {
        txtMoney.text = $"{playerMoney:F2}";
    }

    public void FuelCar(float gasoline)
    {
        playerFuel += gasoline;
        playerMoney -= gasoline * literPrice;
        txtMoney.text = $"{playerMoney:F2}";
        txtGas.text = playerFuel.ToString();
    }

    public void BurnFuel(float gasInput)
    {
        if (!isGamePaused)
        {
            txtGas.text = $"{playerFuel:F2}";
            playerFuel -= fuelBurn * fuelBurnMultiplier;
        }
    }

    private void NewRating(int rating)
    {
        ratingSum += rating;
        totalClients++;
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
        txtMoney.text = $"{playerMoney:F2}";
    }


}
