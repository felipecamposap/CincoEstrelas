using System.Threading;
using UnityEngine;
using UnityEngine.UI;


public class ClientTest : MonoBehaviour
{
    [SerializeField] private Database db;
    [SerializeField] private GameObject clientPrefab;
    [SerializeField] private GameObject destinyPrefab;
    [SerializeField] private Transform[] blockParent;
    [SerializeField] private Transform[] blockPos;
    [SerializeField] private int blocksVariation;
    [Header("Propriedades da Hud: ")]
    [SerializeField]
    private Text txtClientName;
    [SerializeField] private Text txtClientPay;
    [SerializeField] private Slider sldClientRating;
    

    //----
    private string clientName;
    private int rating;
    private float pay;

    private void Start()
    {
        blockPos = new Transform[blockParent.Length * 2];
        var countPos = 0;
        for (var i = 0; i < blockParent.Length;i++)
            for (var j = 0; j < 2; j++)
            {
                blockPos[countPos] = blockParent[i].GetChild(j);
                countPos++;
            }
    }

    public void FindClient()
    {
        clientName = db.nameClients[Random.Range(0, db.nameClients.Length)];
        rating = Random.Range(2, 6);
        pay = Random.Range(5f, 10f) + (rating - 2f);
        Debug.Log("name: " + clientName + "| rating : " + rating + "| pay: " + pay);
        txtClientName.text = clientName;
        txtClientPay.text = $"R${pay:F2}";
        sldClientRating.value = rating;
    }

    [ContextMenu("Spawn Client")]
    public void SpawnClient()
    {
        var indexClient = 0;
        var indexDesty = 0;
        SetIndex(out indexClient, out indexDesty);
        // -----  Instanciando client  ----->>
        InstanciateObject(clientPrefab, indexClient, 0);
        // -----  Instanciando Destino ----->>
        InstanciateObject(destinyPrefab, indexDesty, 1);
        GameController.controller.StartClientTime();
    }

    private void InstanciateObject(GameObject _instObj, int _index, int gcIndex)
    {
        Vector3 pos1 = blockPos[_index*2].position;
        Vector3 pos2 = blockPos[_index * 2 + 1].position;
        Quaternion rot = blockPos[_index * 2].rotation;
        //if ((_index + 1) % blocksVariation == 0)
        //    pos2 = blockPos[_index - 1].position;
        //else
        //    pos2 = blockPos[_index + 1].position;
        var rangeSpawn = Random.Range(0.2f, 0.8f);
        var clone = Instantiate(_instObj, Vector3.Lerp(pos1, pos2, rangeSpawn), rot);
        GameController.controller.minimapaAlvo[gcIndex] = clone.transform;
        if(gcIndex == 0)
        {
            clone.GetComponent<Client>().SetAttributes(clientName, rating, pay);
        }
    }

    private void SetIndex(out int _indexClient, out int _indexDesty)
    {
        _indexClient = Random.Range(0, blockParent.Length);
        _indexDesty = Random.Range(0, blockParent.Length);
        int newDestiny = Random.Range(0, 2) * 2 - 1;

        while (Vector3.Distance(blockParent[_indexDesty].position, blockParent[_indexClient].position) <= 450){
            _indexDesty += newDestiny;
            if(_indexDesty >= blockParent.Length)
                _indexDesty = 0;
            else if(_indexDesty < 0)
                _indexDesty = blockParent.Length-1;
        }
    }
}
