using UnityEngine.UI;
using UnityEngine;


public class Mecanica : MonoBehaviour
{
    [Header("0 - atual | 1 - Após conserto")] [SerializeField]
    private Image[] fillImages; // 0 - Atual | 1 - Conserto

    [SerializeField] private Slider sldIntegrity;
    [SerializeField] private Text txtPrice;
    [SerializeField] private Button btnPagar;
    [SerializeField] private Text txtPagar;

    private float price;
    private float brokenAmount, brokenAmountPerCent, maxLife, currentLife, repairPerCent;

    [Header("Upgrades")] [SerializeField] private Button[] btnUpgrade; // 0 - Motor | 1 - Freio
    [SerializeField] private Text[] txtUpgrade;
    [SerializeField] private Text[] priceUpgrade;

    // ----- Variaveis de Controle ----- \\
    private float playerMoney;
    private float motorPower;
    private float upgradePrice;
    private float brakePower;


    private void OnEnable()
    {
        UpdateUI();
        SetControlVariables();
        GameController.controller.Interaction(true);
    }

    public void SetControlVariables()
    {
        playerMoney = GameController.controller.PlayerMoney;
        upgradePrice = GameController.upgradePrice;
        motorPower = GameController.controller.player.motorPower;
        // ------ Motor upgrade
        bool canUpgrade = motorPower < 2000 && playerMoney >= upgradePrice;
        if (canUpgrade)
                txtUpgrade[0].color = Color.white;
            else
                txtUpgrade[0].color = Color.black;
            btnUpgrade[0].interactable = canUpgrade;
        priceUpgrade[0].text = $"Preço: R${GameController.upgradePrice:R2}";
        // ------ brake upgrade
        brakePower = GameController.controller.player.brakePower;
        canUpgrade = brakePower < 0.7f && playerMoney >= upgradePrice / 2;
        if (canUpgrade)
            txtUpgrade[1].color = Color.white;
        else
            txtUpgrade[1].color = Color.black;
        btnUpgrade[1].interactable = canUpgrade;
        priceUpgrade[1].text = $"Preço: R${GameController.upgradePrice/2:R2}";
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
        price = (brokenAmount * _value) * 2f; // preço multiplier
        repairPerCent = _value;
        fillImages[1].fillAmount = brokenAmountPerCent + (1 - brokenAmountPerCent) * _value;
        txtPrice.text = $"R${price:F2}";
        if (price > playerMoney)
        {
            btnPagar.interactable = false;
            txtPagar.color = Color.black;
            Debug.Log("AAAAAAAAAAAAAAAA");
        }
        else
        {
            btnPagar.interactable = true;
            txtPagar.color = Color.white;
        }
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
        //GameController.controller.GetPaid(-upgradePrice, false);
        GameController.controller.ChangeMoney(-upgradePrice);
        GameController.controller.player.UpgradeMotor();
        SetControlVariables();
    }
    
    public void UpgradeBrake()
    {
        //GameController.controller.GetPaid(-upgradePrice / 2, false);
        GameController.controller.ChangeMoney(-upgradePrice/2);
        GameController.controller.player.UpgradeBrake();
        SetControlVariables();
    }
}