using UnityEngine;

public class ListClients : MonoBehaviour
{
    [SerializeField] ClientsParameters head;
    [SerializeField] ClientsParameters tail;
    public int totalClients { get; set; }

    private void Start()
    {
        GameController.controller.listClients = this;
    }

    private ListClients()
    {
        head = null;
        tail = null;
    }

    public void Insert(ClientsParameters _client)
    {
        if(head == null)
        {
            head = _client;
            tail = head;

        }else{
            ClientsParameters count = head;
            while(count.next != null)
                count = count.next;
            count.next =_client;
            tail = _client;
        }
        totalClients++;

    }

    public void GetClient(int _value, ClientsParameters _client)
    {
        ClientsParameters countClient = head;
        int count = 1;
        while (count != _value){
            countClient = countClient.next;
            count++;
        }
        _client = countClient;
    }

}
