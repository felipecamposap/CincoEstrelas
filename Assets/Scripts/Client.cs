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

    private void Start()
    {
        mainCamera = Camera.main.transform;

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
        }
        if (touchPlayer == 0 || touchPlayer == 3)
        {
            var targetPos = target.position;
            targetPos.y = 1.4f;
            transform.position = Vector3.Lerp(transform.position, targetPos, speed * Time.deltaTime);
            if (Vector3.Distance(transform.position, target.position) <= 2.5f)
            {
                if (target.CompareTag("Player"))
                {
                    GameController.controller.uiController.MostrarEstrelaCorrida();
                    GameController.controller.penalty = 0;
                    touchPlayer = 2;
                    transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                    transform.rotation = target.transform.localRotation;
                    //transform.localRotation = new Quaternion(1, transform.localRotation.y, transform.localRotation.z, 1);
                    GetComponent<BoxCollider>().size = new Vector3(2, 2, 2);
                    transform.parent = target.transform;
                    distanceParticle.SetActive(false);
                }
                else
                {
                    Destroy(gameObject);
                    Destroy(target.gameObject);
                }

            }

        }

        if (touchPlayer == -1 && isTouchingPlayer)
            CheckDistance();

    }

    private void CheckDistance()
    {
        if (Vector3.Distance(target.position, transform.position) >= 10f)
            GameController.controller.ResetClient();
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
            ArriveDestination(other.transform);

        }
        else if (other.CompareTag("Player") && touchPlayer == -1 && !isTouchingPlayer)
        {
            canvas.gameObject.SetActive(true);
            isTouchingPlayer = true;
            password.gameObject.SetActive(true);
            GameController.controller.PasswordClient();
            password.text = GameController.controller.passwordClient.ToString();
            GameController.controller.uiController.CellPhoneAnimation(6);
            target = other.transform;
            distanceParticle?.SetActive(true);

        }

    }

    public void ArriveDestination(Transform _target)
    {
        GameController.controller.GetPaid(clientPayment, true);
        transform.parent = null;
        target = _target;
        touchPlayer++;
        GameController.controller.AddHistoryClient(new ClientsParameters(clientName, clientRating, clientPayment));
        GameController.controller.alvoMinimapa.index++;
        GameController.controller.uiController.EsconderEstrelaCorrida();
    }

    private void OnTriggerStay(Collider other)
    {
        if (touchPlayer == 0 && other.CompareTag("Player"))
        {
            coll.enabled = false;
        }

    }

}
