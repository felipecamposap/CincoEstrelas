using UnityEngine;
using UnityEngine.UI;


public class ClientTest : MonoBehaviour
{
    [SerializeField] Database db;
    [SerializeField] GameObject clientPrefab;
    [SerializeField] GameObject destinyPrefab;
    [SerializeField] Transform[] blockPos;
    [Header("Propriedades da Hud: ")]
    [SerializeField] Text txtClientName;
    [SerializeField] Text txtClientPay;
    [SerializeField] Slider sldClientRating;

    //----
    private string clientName;
    private int rating;
    private float pay;

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
        int indexClient = 0;
        int indexDesty = 0;
        SetIndex(out indexClient, out indexDesty);
        // -----  Instanciando client  ----->>
        InstanciateObject(clientPrefab, indexClient, 0);
        // -----  Instanciando Destino ----->>
        InstanciateObject(destinyPrefab, indexDesty, 1);


    }

    private void InstanciateObject(GameObject _instObj, int _index, int gcIndex)
    {
        Vector3 pos1 = blockPos[_index].position;
        Vector3 pos2;
        if ((_index + 1) % 4 == 0)
            pos2 = blockPos[_index - 3].position;
        else
            pos2 = blockPos[_index + 1].position;
        float rangeSpawn = Random.Range(0.2f, 0.8f);
        GameObject clone = Instantiate(_instObj, Vector3.Lerp(pos1, pos2, rangeSpawn), Quaternion.identity);
        GameController.controller.minimapaAlvo[gcIndex] = clone.transform;
        if(gcIndex == 0)
        {
            clone.GetComponent<Client>().SetAttributes(clientName, rating, pay);
        }
    }

    private void SetIndex(out int _indexClient, out int _indexDesty)
    {
        int numBlocks = Random.Range(0, blockPos.Length / 4);
        _indexClient = Random.Range(0, 4) + numBlocks * 4;
        _indexDesty = Random.Range(0, blockPos.Length / 4);
        if (_indexDesty == numBlocks){
            _indexDesty++;
            if (_indexDesty > (blockPos.Length / 4) - 1)
                _indexDesty = 0;
        }
        _indexDesty = Random.Range(0, 4) + _indexDesty * 4;

    }

}
