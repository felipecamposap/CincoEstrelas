using UnityEngine;

public class ListClients : MonoBehaviour
{
    [SerializeField] ClientsParameters head;
    [SerializeField] ClientsParameters tail;
    public int totalClients { get; set; }

    private void Awake()
    {
        GameController.controller.listClients = this;
    }

    public ListClients()
    {
        head = null;
        tail = null;
    }

    public void Insert(ClientsParameters _client)
    {
        if (head == null)
        {
            head = _client;
            tail = head;
        }
        else
        {
            ClientsParameters count = head;
            while (count.next != null)
                count = count.next;
            count.next = _client;
            tail = _client;
        }
        totalClients++;
    }

    public void Insert(ClientsParameters _client, byte sort)  // 0: default order | 1: money | 2: client rating | 3: alphabetical order
    {
        if (head == null)
        {
            head = _client;
            tail = head;
        }
        else
        {

            switch (sort)
            {
                case 0:

                    ClientsParameters count = head;
                    while (count.next != null)
                        count = count.next;
                    count.next = _client;
                    tail = _client;
                    break;
                case 1:
                    ClientsParameters count = head;
                    if (count.next != null)
                    {
                        if (count.paid > _client.paid)
                        {
                            count = count.next;
                        }
                        else
                        {
                            count.next = _client;
                        }
                    }
                    break;
                case 2:
            }
        }
        totalClients++;
    }

    public void GetClient(int _value, out ClientsParameters _client)
    {
        //Debug.Log(head.clientName);
        ClientsParameters countClient = head;
        int count = 1;
        while (count != _value)
        {
            countClient = countClient.next;
            count++;
        }
        //Debug.Log(countClient.clientName);
        _client = countClient;
    }

}
