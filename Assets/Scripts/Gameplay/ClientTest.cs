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
        var pos1 = blockPos[_index].position;
        Vector3 pos2;
        if ((_index + 1) % blocksVariation == 0)
            pos2 = blockPos[_index - 1].position;
        else
            pos2 = blockPos[_index + 1].position;
        var rangeSpawn = Random.Range(0.2f, 0.8f);
        var clone = Instantiate(_instObj, Vector3.Lerp(pos1, pos2, rangeSpawn), Quaternion.identity);
        GameController.controller.minimapaAlvo[gcIndex] = clone.transform;
        if(gcIndex == 0)
        {
            clone.GetComponent<Client>().SetAttributes(clientName, rating, pay);
        }
    }

    private void SetIndex(out int _indexClient, out int _indexDesty)
    {
        var numBlocks = Random.Range(0, blockPos.Length / blocksVariation);
        _indexClient = Random.Range(0, blocksVariation) + numBlocks;
        _indexDesty = Random.Range(0, blockPos.Length / blocksVariation);
        if ((_indexDesty == numBlocks || _indexDesty == _indexClient)){
            _indexDesty++;
            if (_indexDesty > (blockPos.Length / blocksVariation) - 1)
                _indexDesty = 0;
        }

        while(Vector3.Distance(blockPos[_indexDesty].position, blockPos[_indexClient].position) <= 450){
            _indexDesty++;
            if(_indexDesty >= blockPos.Length)
                _indexDesty = 0;
        }
        _indexDesty = Random.Range(0, blocksVariation) + _indexDesty;

    }

}
