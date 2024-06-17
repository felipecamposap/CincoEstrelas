using UnityEngine;
using UnityEngine.UI;


public class BillsBank : MonoBehaviour
{
    [SerializeField] private Text currentMoney;
    [SerializeField] private Text waterText;
    [SerializeField] private Text lightText;
    [SerializeField] private Text internetText;
    [SerializeField] private Text mobileText;
    [SerializeField] private Text iptuText;
    [SerializeField] private Text foodText;


    // Start is called before the first frame update
    private void OnEnable()
    {
        currentMoney.text = $"R${GameController.controller.PlayerMoney:F2}";
        waterText.text = $"Água = -R${GameController.controller.vwater:F2}";
        lightText.text = $"Luz = -R${GameController.controller.vlight:F2}";
        internetText.text = $"Internet = -R${GameController.controller.internet:F2}";
        mobileText.text = $"3g = -R${GameController.controller.netMobile:F2}";
        iptuText.text = $"Iptu = -R${GameController.controller.iptu:F2}";
        foodText.text = $"Comida = -R${GameController.controller.food:F2}";
    }
}
