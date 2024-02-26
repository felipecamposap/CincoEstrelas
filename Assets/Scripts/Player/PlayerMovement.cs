using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    float brakeInput;
    float steeringInput;
    [SerializeField] float maxSteeringAngle = 45;
    [SerializeField] float motorPower = 3000.0f; // Adjust the value as needed
    [SerializeField] float brakePower = 4000.0f; // Adjust the value as needed
    [SerializeField] float gasDrag = 0.2f, idleDrag = 0.5f, brakeDrag = 1.5f, brakeThreshold = 2f;
    float speed;
    public AnimationCurve steeringCurve;
    private Vector3 localVelocity;
    [SerializeField] GameObject danoFaisca, luzesFreio;
    [SerializeField] GameObject vitoriaEfeito;
    public bool inGame;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        GameController.controller.player = this;
    }

    void Update()
    {
        if (!inGame)
        {
            rb.drag = 100;
            ApplyBrake();
            return;
        }
        CheckInput();
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        speed = rb.velocity.magnitude * 0.65f;
        GameController.controller.uiController.Velocity(speed / 30);
        ApplyMovement();
        ApplyMotor();
        ApplyBrake();
        ApplySteering();
        ApplyWheelPos();
        //Debug.Log(brakeInput);
    }

    private void FixedUpdate()
    {
        if (inGame && Input.GetAxis("Vertical") != 0 && !GameController.controller.trapacas[1] && GameController.controller.PlayerFuel > 0)
        {
            GameController.controller.BurnFuel(gasInput);
        }
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal"); 
    }

    void ApplyMovement()
    {
        if (gasInput < 0) //re
        {
            if (localVelocity.z > brakeThreshold) //carro andando para frente
            {
                luzesFreio.SetActive(true);
                //Debug.Log("Frear");
                brakeInput = 1;
                rb.drag = brakeDrag;
            }
            else
            {
                luzesFreio.SetActive(false);
                //Debug.Log("Re");
                brakeInput = 0;
                rb.drag = gasDrag;
            }
        }
        else if (gasInput == 0) //carro solto
        {
            luzesFreio.SetActive(false);
            brakeInput = 0.05f;
            rb.drag = idleDrag;
        }
        else //acelerando
        {
            if (localVelocity.z < -brakeThreshold) //carro andando para frente
            {
                luzesFreio.SetActive(true);
                //Debug.Log("Frear");
                brakeInput = 1;
                rb.drag = brakeDrag;
            }
            else
            {
                luzesFreio.SetActive(false);
                //Debug.Log("Acelerar");
                brakeInput = 0;
                rb.drag = gasDrag;
            }
        }

    }

    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.FLWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.BRWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.BLWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
    }

    void ApplyMotor()
    {
        colliders.BRWheel.motorTorque = motorPower * gasInput; // Adjust the value as needed
        colliders.BLWheel.motorTorque = motorPower * gasInput; // Adjust the value as needed
    }

    void ApplySteering()
    {
        float steeringSensitivity = 6f; // Adjust the value as needed
        float steeringAngle = steeringSensitivity * steeringInput * steeringCurve.Evaluate(speed);
        steeringAngle = Mathf.Clamp(steeringAngle, -maxSteeringAngle, maxSteeringAngle);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPos()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.BRWheel, wheelMeshes.BRWheel);
        UpdateWheel(colliders.BLWheel, wheelMeshes.BLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion rot;
        Vector3 pos;
        coll.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;

    }

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        if (inGame) {
            float damageValue = rb.velocity.magnitude * 1.5f;
            yield return new WaitForSeconds(0.1f);
            damageValue -= rb.velocity.magnitude;
            if (collision.gameObject.CompareTag("Damagable") || collision.gameObject.CompareTag("NPC"))
            {
                ContactPoint contact = collision.contacts[0];
                Instantiate(danoFaisca, contact.point, Quaternion.identity);
                GameController.controller.penalty += 1;
                if (!GameController.controller.trapacas[0])
                    GameController.controller.Damage((damageValue * 10f));
            }
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Calcada"))
        {
            GameController.controller.penalty += 1;
        }
    }

    public void PlayerVictory()
    {
        vitoriaEfeito.SetActive(true);
        inGame = false;
    }

}

[System.Serializable]
public class WheelColliders
{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider BRWheel;
    public WheelCollider BLWheel;
}

[System.Serializable]
public class WheelMeshes
{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer BRWheel;
    public MeshRenderer BLWheel;
}