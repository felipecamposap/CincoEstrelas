using UnityEngine;

public class ListClients
{
    [SerializeField] ClientsParameters head;
    [SerializeField] ClientsParameters tail;
    public int totalClients { get; set; }

    private void Awake()
    {
        if (GameController.controller.listClients != null)
            GameController.controller.listClients = this;
    }

    public ListClients()
    {
        head = null;
        tail = null;
    }

    public void Insert(ClientsParameters client)
    {
        ClientsParameters _client = new ClientsParameters(client.clientName, client.rating, client.paid);
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

    public void Insert(ClientsParameters client, int sort)  // 0: default order | 1: money | 2: client rating | 3: alphabetical order
    {
        ClientsParameters _client = new ClientsParameters(client.clientName, client.rating, client.paid);
        if (head == null)
        {
            head = _client;
            tail = head;
        }
        else
        {
            ClientsParameters count = head;
            ClientsParameters countAux = null;
            switch (sort)
            {
                case 0:
                    while (count.next != null)
                        count = count.next;
                    count.next = _client;
                    tail = _client;
                    break;
                case 1:
                    while (_client != null)
                    {

                        if (count != null && count.paid > _client.paid)
                        {
                            countAux = count;
                            count = count.next;
                        }
                        else
                        {
                            if (count == head)
                            {
                                _client.next = head;
                                head = _client;
                                break;
                            }
                            else
                            {
                                if (countAux.next != null)
                                    _client.next = countAux.next;
                                countAux.next = _client;
                                if (countAux == tail)
                                    tail = _client;
                                break;
                            }
                        }
                    }
                    break;
                case 2:
                    while (_client != null)
                    {
                        if (count != null && count.rating > _client.rating)
                        {
                            countAux = count;
                            count = count.next;
                        }
                        else if (count == head)
                        {
                            _client.next = head;
                            head = _client;
                            break;
                        }
                        else
                        {
                            if (countAux.next != null)
                                _client.next = countAux.next;
                            countAux.next = _client;
                            if (countAux == tail)
                                tail = _client;
                            break;
                        }
                    }
                    break;
                case 3:
                    while (_client != null)
                    {
                        if (count != null && string.Compare(count.clientName, _client.clientName) == -1)
                        {
                            //Debug.Log(countAux + "B");
                            countAux = count;
                            count = count.next;
                        }
                        else if (count == head)
                        {
                            _client.next = head;
                            head = _client;
                            break;
                        }
                        else
                        {
                            //Debug.Log(countAux.clientName + " -> " + _client.clientName);
                            //Debug.Log(string.Compare(countAux.clientName, _client.clientName));

                            if (count != null)
                            {
                                //Debug.Log(count.clientName);
                                //Debug.Log(countAux.next.clientName);
                            }
                            if (countAux.next != null)
                                _client.next = countAux.next;
                            countAux.next = _client;
                            if (countAux == tail)
                                tail = _client;
                            break;
                        }
                    }
                    break;
            }
        }
        totalClients++;
    }

    public void GetClient(int _value, out ClientsParameters _client)
    {
        //Debug.Log("Head: " + head.clientName);

        ClientsParameters countClient = head;
        int count = 0;
        while (count != _value)
        {

            countClient = countClient.next;
            count++;
        }
        //Debug.Log("Contador: " + countClient.clientName);
        //Debug.Log(countClient.clientName);
        _client = new ClientsParameters(countClient.clientName, countClient.rating, countClient.paid); ;
    }
}
