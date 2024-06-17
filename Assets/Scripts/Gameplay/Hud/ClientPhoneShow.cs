using UnityEngine;
using UnityEngine.UI;

public class ClientPhoneShow : MonoBehaviour
{
    [SerializeField] private Text clientName;
    [SerializeField] private Slider clientRating;
    [SerializeField] private Text clientPay;

    public void SetParameters(string _name, int _rating, float _pay)
    {
        clientName.text = _name;
        clientRating.value = _rating;
        clientPay.text = $"R${_pay:F2}";
    }
}

