using UnityEngine;
using UnityEngine.UI;


public class GasStation : MonoBehaviour
{
    [SerializeField] private Slider sldGas;
    [SerializeField] private Text txtLiters;
    [SerializeField] private Text txtPrice;


    private void OnEnable()
    {
        //if (other.CompareTag("Player"))
        //{
        //    if (GameController.controller.PlayerMoney > 0)
        //    {
        //        GameController.controller.ToggleCursor(true);
        //        GameController.controller.SetGamePaused(true);
        //        sldGas.maxValue = GameController.controller.AvailableFuelSpace < GameController.controller.BuyableLiters ? GameController.controller.AvailableFuelSpace : GameController.controller.BuyableLiters;
        //        gasUI.SetActive(true);
        //    }
        //    else
        //    {
        //        throw new System.Exception("Não há dinheiro suficiente para abastecer");
        //    }
        //}
        GameController.controller.Interaction(true);
        sldGas.maxValue = GameController.controller.AvailableFuelSpace < GameController.controller.BuyableLiters ? GameController.controller.AvailableFuelSpace : GameController.controller.BuyableLiters;
        sldGas.value = 0;
        if(sldGas.maxValue > 0)
            UpdateUI(0);
    }

    public void UpdateUI(float value)
    {
        var price = value * GameController.literPrice;

        txtLiters.text = $"{value:F2}";
        txtPrice.text = $"R${price:F2}";
    }

    public void PayGas()
    {
        GameController.controller.FuelCar(sldGas.value);
        CloseUI();
    }

    public void CloseUI()
    {
        GameController.controller.Interaction(false);
    }
}
