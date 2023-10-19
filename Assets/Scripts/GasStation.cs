using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GasStation : MonoBehaviour
{
    [SerializeField] private Slider sldGas;
    [SerializeField] private TMP_Text txtLiters;
    [SerializeField] private TMP_Text txtPrice;
    [SerializeField] private Button btnOK;
    [SerializeField] private GameObject gasUI;
    [SerializeField] private GameController gc;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(gc.PlayerMoney > 0)
            {
                gc.ToggleCursor(true);
                gc.SetGamePaused(true);
                sldGas.maxValue = gc.AvailableFuelSpace < gc.BuyableLiters ? gc.AvailableFuelSpace : gc.BuyableLiters;
                gasUI.SetActive(true);
            }
            else
            {
                throw new System.Exception("N�o h� dinheiro suficiente para abastecer");
            }
        }
    }

    public void UpdateUI(float value)
    {
        float price = value * gc.literPrice;

        txtLiters.text = $"{value:F2}";
        txtPrice.text = $"{price:F2}";
    }

    public void CloseUI()
    {
        gc.ToggleCursor(false);
        gc.FuelCar(sldGas.value);
        gc.SetGamePaused(false);
        gasUI.SetActive(false);
    }
}
