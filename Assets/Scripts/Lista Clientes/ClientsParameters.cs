using UnityEngine;

public class ClientsParameters
{
    public ClientsParameters next { get; set; }
    public string clientName { get; set; }
    public int rating { get; set; }
    public float paid { get; set; }

    public ClientsParameters(string _clientName, int _rating, float _paid)
    {
        this.clientName = _clientName;
        this.rating = _rating;
        this.paid = _paid;
    }

}
