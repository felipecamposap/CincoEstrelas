using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ClientsSpawner : MonoBehaviour
{
    
    [SerializeField] private Transform[] locations;
    [SerializeField] private GameObject client;
    [SerializeField] private GameObject clientDestination;
    //[SerializeField] private Animator CellPhoneUI;
    [SerializeField] private string[] clientNames;
    [SerializeField] private Text txtClient;
    [SerializeField] private Slider clientRating;
    [SerializeField] private Text txtClientPay;
    float currentClientPay = 0;

    private int indexName;

    public void ClientSpecifications()
    {
        indexName = Random.Range(0, clientNames.Length);
        txtClient.text = clientNames[indexName];
        int currentClientRating = Random.Range(2, 6);
        clientRating.value = currentClientRating;
        currentClientPay = Random.Range(5f, 10f) + (currentClientRating - 2);
        txtClientPay.text = $"R${currentClientPay:F2}";
        //CellPhoneUI.SetBool("Activate", true);
    }

    public void CloseCellPhone()
    {
        //CellPhoneUI.SetBool("Activate", false);
    }

    public void ShowClient(){
        Vector3 pos;
        int rand = Random.Range(0, 2);
        int i = Random.Range(0,locations.Length);
        pos = locations[i].position;
        //Debug.Log("Before: " + pos);
        if (Random.Range(0,2) == 0){
            pos.x += ((locations[i].localScale.x / 2) * (rand * 2 - 1));
            pos.z += Random.Range(-(locations[i].localScale.z * 0.1f), (locations[i].localScale.z * 0.1f));
            //Debug.Log("Random Z" + (rand * 2 - 1));
            //Debug.Log("posX: " + pos.x);
            //Debug.Log("posZ: " + pos.z);
            ShowClientDestination(0, i);
        }else{
            pos.z += ((locations[i].localScale.z / 2) * (rand * 2 - 1));
            pos.x += Random.Range(-(locations[i].localScale.x * 0.1f), (locations[i].localScale.x * 0.1f));
            //Debug.Log("Random X : " + (rand * 2 - 1));
            //Debug.Log("posX: " + pos.x);
            //Debug.Log("posZ: " + pos.z);
            ShowClientDestination(1, i);
        }
        //Debug.Log("After: " + pos);
        GameObject clone = Instantiate(client, pos, Quaternion.identity);
        Client scriptClient = clone.GetComponentInChildren<Client>();
        scriptClient.payment = currentClientPay;
        scriptClient.clientName = clientNames[indexName];
        //GameController.controller.uiController.CellPhoneAnimation(1); // abaixar celular
    }

    public void ShowClientDestination(int loc, int ind)
    {
        Vector3 pos;
        int rand = Random.Range(0, 2);
        pos = locations[ind].position;
        if (loc == 1)
        {
            pos.x += ((locations[ind].localScale.x / 2) * (rand * 2 - 1));
            pos.z += Random.Range(-(locations[ind].localScale.z * 0.1f), (locations[ind].localScale.z * 0.1f));
            //Debug.Log("Random Z" + (rand * 2 - 1));
            //Debug.Log("posX: " + pos.x);
            //Debug.Log("posZ: " + pos.z);
        }else{
            pos.z += ((locations[ind].localScale.z / 2) * (rand * 2 - 1));
            pos.x += Random.Range(-(locations[ind].localScale.x * 0.1f), (locations[ind].localScale.x * 0.1f));
            //Debug.Log("Random X : " + (rand * 2 - 1));
            //Debug.Log("posX: " + pos.x);
            //Debug.Log("posZ: " + pos.z);
        }
        Instantiate(clientDestination, pos, Quaternion.identity);
        //CloseCellPhone();
    }

}
