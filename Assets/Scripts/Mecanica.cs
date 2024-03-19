using UnityEngine.UI;
using UnityEngine;


public class Mecanica : MonoBehaviour
{
    [Header("0 - atual | 1 - Após conserto")]
    [SerializeField] Image[] fillImages; // 0 - Atual | 1 - Conserto
    [SerializeField] Slider sldIntegrity;
    [SerializeField] Text txtPrice;
    [SerializeField] Button btnPagar;

    private float price;
    private float brokenAmount, brokenAmountPerCent, maxLife, currentLife, repairPerCent;


    private void OnEnable()
    {
        UpdateUI();

        GameController.controller.Interaction(true);

        //AttUI();
    }

    public void UpdateUI()
    {
        maxLife = GameController.controller.carIntegrityMax;
        currentLife = GameController.controller.carIntegrityCurrent;
        brokenAmount = maxLife - currentLife;
        brokenAmountPerCent = currentLife / maxLife;
        fillImages[0].fillAmount = brokenAmountPerCent;
        fillImages[1].fillAmount = brokenAmountPerCent;
        repairPerCent = 0;
        sldIntegrity.value = 0;
    }

    public void BuyIntegrity()
    {
        GameController.controller.RecoverIntegrity(brokenAmount * repairPerCent, -price);
        UpdateUI();

    }

    public void RepairSlider(float _value)
    {
        price = (brokenAmount * _value) * 5f; // preço multiplier
        repairPerCent = _value;
        fillImages[1].fillAmount = brokenAmountPerCent + (1 - brokenAmountPerCent) * _value;
        txtPrice.text = $"R${price:F2}";
        if (price > GameController.controller.PlayerMoney)
            btnPagar.interactable = false;
        else
            btnPagar.interactable = true;
    }

    public void CloseUI()
    {
        GameController.controller.Interaction(false);
    }

    public void AttUI()
    {
        for (int i = 0; i < fillImages.Length; i++)
            fillImages[i].fillAmount = maxLife;

        //sldIntegrity.maxValue = GameController.controller.carIntegrityMax - GameController.controller.carIntegrityCurrent;
        fillImages[0].fillAmount = GameController.controller.carIntegrityCurrent;
    }
}