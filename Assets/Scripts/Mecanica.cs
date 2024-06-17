using UnityEngine.UI;
using UnityEngine;


public class Mecanica : MonoBehaviour
{
    [Header("0 - atual | 1 - Após conserto")] [SerializeField]
    private Image[] fillImages; // 0 - Atual | 1 - Conserto

    [SerializeField] private Slider sldIntegrity;
    [SerializeField] private Text txtPrice;
    [SerializeField] private Button btnPagar;

    private float price;
    private float brokenAmount, brokenAmountPerCent, maxLife, currentLife, repairPerCent;

    [Header("Upgrades")] [SerializeField] private Button[] btnUpgrade; // 0 - Motor | 1 - Direcao
    [SerializeField] private Text priceUpgradeMotor;

    // ----- Variaveis de Controle ----- \\
    private float playerMoney;
    private float motorPower;
    private float motorUpgradePrice;


    private void OnEnable()
    {
        UpdateUI();
        SetControlVariables();
        GameController.controller.Interaction(true);
    }

    public void SetControlVariables()
    {
        playerMoney = GameController.controller.PlayerMoney;
        motorUpgradePrice = GameController.motorUpgradePrice;
        motorPower = GameController.controller.player.motorPower;
        priceUpgradeMotor.text = $"Preço: R${GameController.motorUpgradePrice}:R2";

        btnUpgrade[0].interactable = motorPower < 2000 && playerMoney > motorUpgradePrice;
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
        var player = GameController.controller.player;
        GameController.controller.RecoverIntegrity(brokenAmount * repairPerCent, -price);
        if (GameController.controller.carIntegrityCurrent > GameController.controller.carIntegrityMax / 4)
        {
            player.ToggleVFX(player.damageVFX, false);
            
        }
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
        for (var i = 0; i < fillImages.Length; i++)
            fillImages[i].fillAmount = maxLife;

        //sldIntegrity.maxValue = GameController.controller.carIntegrityMax - GameController.controller.carIntegrityCurrent;
        fillImages[0].fillAmount = GameController.controller.carIntegrityCurrent;
    }

    public void UpgradeMotor()
    {
        GameController.controller.GetPaid(motorUpgradePrice, false);
        GameController.controller.player.UpgradeMotor();
        SetControlVariables();
    }
}