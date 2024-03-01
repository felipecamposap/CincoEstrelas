using UnityEngine.UI;
using UnityEngine;


public class Mecanica : MonoBehaviour
{
    [Header("0 - Interativo | 1 - atual | 2 - Após conserto")]
    [SerializeField] Slider[] sldIntegrity; // 0 - slider Interativo | 1 - integridade atual | 2 - integridade após concerto
    [SerializeField] Text txtPrice;
    [SerializeField] Button btnPagar;

    private float price;


    private void OnEnable()
    {
        GameController.controller.Interaction(true);
        AttUI();
    }

    public void UpdateUI()
    {
        price = sldIntegrity[0].value * 11.5f;
        sldIntegrity[2].value = GameController.controller.carIntegrityCurrent + sldIntegrity[0].value;
        txtPrice.text = $"R${price:F2}";
        if (price > GameController.controller.PlayerMoney)
            btnPagar.interactable = false;
        else
            btnPagar.interactable = true;
    }

    public void BuyIntegrity()
    {
        if(price < GameController.controller.PlayerMoney)
            GameController.controller.RecoverIntegrity(sldIntegrity[0].value, -price);

    }

    public void CloseUI()
    {
        GameController.controller.Interaction(false);
    }

    public void AttUI()
    {
        for (int i = 0; i < sldIntegrity.Length; i++)
            sldIntegrity[i].maxValue = GameController.controller.carIntegrityMax;

        sldIntegrity[0].maxValue = GameController.controller.carIntegrityMax - GameController.controller.carIntegrityCurrent;
        sldIntegrity[1].value = GameController.controller.carIntegrityCurrent;
    }
}