using UnityEngine;
using UnityEngine.UI;


public class Client : MonoBehaviour
{
    [Header("Client Parameters: ")]
    [SerializeField] private string clientName;
    [SerializeField] private int clientRating;
    [SerializeField] private float clientPayment;

    [Header("Others: ")]
    [SerializeField] private int touchPlayer;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Collider coll;
    [SerializeField] private Text password;
    private Transform mainCamera;
    [SerializeField] private Canvas canvas;
    
    private bool isTouchingPlayer = false;

    [SerializeField] private GameObject distanceParticle;
    [SerializeField] private GameObject iconMinimap;

    private int indexTarget;

    private void Start()
    {
        mainCamera = Camera.main.transform;
        target = GameController.controller.player.carDoorPos[0];
        indexTarget = 0;

    }

    private void Update()
    {
        if (canvas.gameObject.activeSelf)
        {
            canvas.transform.LookAt(mainCamera.position);
        }
        if (GameController.controller.passwordCorrect)
        {
            GameController.controller.passwordCorrect = false;
            touchPlayer = 0;
            canvas.gameObject.SetActive(false);
            GameController.controller.uiController.CellPhoneAnimation(3);
            GameController.controller.alvoMinimapa.index++;
            transform.GetChild(0).GetComponent<Animator>().SetInteger("State", 1);
            CheckDoorDistance();

        }
        if (touchPlayer == 0 || touchPlayer == 3)
        {
            var targetPos = target.position;
            targetPos.y = 1.4f;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            Walking();
            if (Vector3.Distance(transform.position, target.position) <= 0.5f)
            {
                if (target.CompareTag("Player"))
                {
                    GameController.controller.uiController.MostrarEstrelaCorrida();
                    GameController.controller.penalty = 0;
                    touchPlayer = 2;
                    transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                    transform.rotation = target.transform.rotation;
                    //transform.localRotation = new Quaternion(1, transform.localRotation.y, transform.localRotation.z, 1);
                    GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);
                    transform.parent = target.transform;
                    distanceParticle?.SetActive(false);
                    StartCoroutine("OpenDoorCar", indexTarget);

                }
                else
                {
                    Destroy(gameObject);
                    Destroy(target.gameObject);
                }

            }

        }

    }

    private void CheckDoorDistance()
    {
        float newDistance = Vector3.Distance(transform.position, GameController.controller.player.carDoorPos[1].position);
        if (Vector3.Distance(transform.position, target.position) > newDistance)
        {
            target = GameController.controller.player.carDoorPos[1];
            indexTarget = 1;
        }
    }
    private System.Collections.IEnumerator OpenDoorCar(int value)
    {
        if(value == 2)
            yield return new WaitForSeconds(2);
        transform.GetChild(0).GetComponent<Animator>().SetInteger("State", (value+1) * 2);
        GameController.controller.player.OpenDoor((value + 1));
        yield return new WaitForSeconds(5.2f);
        GameController.controller.player.inGame = true;
        if (value == 2)
        {
            GameController.controller.ResetClient(); // Deletar Cliente e destino
        }
    }

    private void Walking()
    {
        Vector3 dir = target.position;
        dir.y = transform.position.y;
        transform.LookAt(dir);
        
    }



    public void SetAttributes(string _name, int _rating, float _pay)
    {
        clientName = _name;
        clientRating = _rating;      
        clientPayment = _pay;
        GameController.controller.PasswordClient();
        
    }

    public void GetAttributes(string _clientName, int _clientRating, float _clientPayment)
    {
        _clientName = clientName;
        _clientRating = clientRating;
        _clientPayment = clientPayment;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (touchPlayer == 2 && other.CompareTag("Destination"))
        {
            ArriveDestination();

        }
        else if (other.CompareTag("Player") && touchPlayer == -1 && !isTouchingPlayer)
        {
            canvas.gameObject.SetActive(true);
            isTouchingPlayer = true;
            password.gameObject.SetActive(true);
            GameController.controller.PasswordClient();
            password.text = GameController.controller.passwordClient.ToString();
            GameController.controller.uiController.CellPhoneAnimation(6);
            //target = other.transform;
            GameController.controller.player.inGame = false;
            GameController.controller.minimapaAlvo[1].gameObject.SetActive(true);
            iconMinimap?.SetActive(false);
            distanceParticle?.SetActive(true);
            GameController.controller.StopClientTime();
            
            

        }

    }

    public void ArriveDestination()
    {
        GameController.controller.player.inGame = false;
        GameController.controller.GetPaid(clientPayment, true);
        GameController.controller.AddHistoryClient(new ClientsParameters(clientName, clientRating, clientPayment));
        GameController.controller.uiController.EsconderEstrelaCorrida();
        StartCoroutine("OpenDoorCar", 2);
        //transform.parent = null;
        //target = _target;
        //touchPlayer++;
        //GameController.controller.alvoMinimapa.index++;
    }

    private void OnTriggerStay(Collider other)
    {
        if (touchPlayer == 0 && other.CompareTag("Player"))
        {
            coll.enabled = false;
        }

    }

}
