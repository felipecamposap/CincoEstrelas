using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GasStation : MonoBehaviour
{
    [SerializeField] private Slider sldGas;
    [SerializeField] private Text txtLiters;
    [SerializeField] private Text txtPrice;
    [SerializeField] private Button btnOK;
    [SerializeField] private GameObject gasUI;
    //[SerializeField] private GameController gc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(GameController.controller.PlayerMoney > 0)
            {
                GameController.controller.ToggleCursor(true);
                GameController.controller.SetGamePaused(true);
                sldGas.maxValue = GameController.controller.AvailableFuelSpace < GameController.controller.BuyableLiters ? GameController.controller.AvailableFuelSpace : GameController.controller.BuyableLiters;
                gasUI.SetActive(true);
            }
            else
            {
                throw new System.Exception("Não há dinheiro suficiente para abastecer");
            }
        }
    }

    public void UpdateUI(float value)
    {
        float price = value * GameController.controller.literPrice;

        txtLiters.text = $"{value:F2}";
        txtPrice.text = $"R${price:F2}";
    }

    public void CloseUI()
    {
        GameController.controller.ToggleCursor(false);
        GameController.controller.FuelCar(sldGas.value);
        GameController.controller.SetGamePaused(false);
        gasUI.SetActive(false);
    }
}
