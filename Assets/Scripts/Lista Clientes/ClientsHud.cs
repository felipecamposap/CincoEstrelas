using UnityEngine;
using UnityEngine.UI;

public class ClientsHud : MonoBehaviour
{
    [SerializeField] private Text nameText;
    [SerializeField] private Slider rating;
    [SerializeField] private Text money;


    public void GetClientsInfo(ClientsParameters _client)
    {
        this.nameText.text = _client.clientName;
        this.rating.value = _client.rating;
        this.money.text = $"R${_client.paid:F2}";
    }

}
