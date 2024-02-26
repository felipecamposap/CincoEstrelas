using UnityEngine;
using UnityEngine.UI;

public class ClientPhoneShow : MonoBehaviour
{
    [SerializeField] Text clientName;
    [SerializeField] Slider clientRating;
    [SerializeField] Text clientPay;

    public void SetParameters(string _name, int _rating, float _pay)
    {
        clientName.text = _name;
        clientRating.value = _rating;
        clientPay.text = $"R${_pay:F2}";
    }
}

