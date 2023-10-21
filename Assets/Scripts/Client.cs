using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Client : MonoBehaviour
{
    [SerializeField] private int touchPlayer;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Collider coll;
    [SerializeField] private Text password;

    public string clientName { get; set; }
    public float payment { get; set; }
    //private GameController gc;

    private void Awake()
    {
        //gc = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if(GameController.controller.passwordCorrect){
            touchPlayer = 0;
            GameController.controller.passwordCorrect = false;
            GameController.controller.uiController.CellPhoneAnimation(1);
            password.gameObject.SetActive(false);
        }
        if (touchPlayer == 0 || touchPlayer == 3){
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.position) <= 1){
                if (target.CompareTag("Player"))
                {
                    touchPlayer = 2;
                    transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                    transform.rotation = target.transform.localRotation;
                    //transform.localRotation = new Quaternion(1, transform.localRotation.y, transform.localRotation.z, 1);
                    transform.parent = target.transform;
                }else{
                    Destroy(gameObject);
                    Destroy(target.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (touchPlayer == 2 && other.CompareTag("Destination"))
        {
            GameController.controller.GetPaid(payment);
            int rating = Random.Range(1, 6) * 2;
            GameController.controller.NewRating(rating);
            transform.parent = null;
            target = other.transform;
            touchPlayer++;
            GameController.controller.uiController.CellPhoneAnimation(7);
            ClientsParameters client = new ClientsParameters(clientName, rating, payment);
            GameController.controller.listClients.Insert(client);

        }
        else if (other.CompareTag("Player") && touchPlayer == -1){
            password.gameObject.SetActive(true);
            GameController.controller.PasswordClient();
            password.text = GameController.controller.passwordClient.ToString();
            GameController.controller.uiController.CellPhoneAnimation(6);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (touchPlayer == 0 && other.CompareTag("Player"))
        {
            coll.enabled = false;
            target = other.transform;

        }
    }
}
