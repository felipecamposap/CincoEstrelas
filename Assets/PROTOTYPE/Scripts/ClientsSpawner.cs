using UnityEngine;
using static UnityEditor.PlayerSettings;

public class ClientsSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] locations;
    [SerializeField] private GameObject[] client;

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
        Instantiate(client[0], pos, Quaternion.identity);


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
            Debug.Log("Random Z" + (rand * 2 - 1));
            Debug.Log("posX: " + pos.x);
            Debug.Log("posZ: " + pos.z);
        }else{
            pos.z += ((locations[ind].localScale.z / 2) * (rand * 2 - 1));
            pos.x += Random.Range(-(locations[ind].localScale.x * 0.1f), (locations[ind].localScale.x * 0.1f));
            Debug.Log("Random X : " + (rand * 2 - 1));
            Debug.Log("posX: " + pos.x);
            Debug.Log("posZ: " + pos.z);
        }
        Instantiate(client[1], pos, Quaternion.identity);
    }
}
