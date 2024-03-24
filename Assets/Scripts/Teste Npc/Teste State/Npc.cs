using UnityEditor.UIElements;
using UnityEngine;

public class Npc : MonoBehaviour
{
    IState state;
    [HideInInspector] public Rigidbody rb;
    public float speed;
    public LinkedStreets currentStreet;
    public Vector3 targetPosition; // Você precisa implementar essa função para obter a próxima posição de destino

    [Header("Raycast Properties:")]
    public Transform frontRay;
    public LayerMask layerRay;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        ChangeState(new Accelerate());
        targetPosition = currentStreet.transform.position;
        speed = Random.Range(7f, 9f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        state?.Update();
        
    }

    public void ChangeState(IState _state)
    {
        state?.Exit();
        state = _state;
        state?.Enter(this);
    }

    public void GetNextDestination()
    {
        if ((transform.position - currentStreet.transform.position).magnitude < 2f)
        {
            int index = Random.Range(0, currentStreet.connectedStreets.Length);
            currentStreet = currentStreet.connectedStreets[index];
        }
        targetPosition = currentStreet.transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Semaforo"))
            state?.TouchTrigger();
        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Semaforo"))
            state?.ExitTrigger();
    }
}
