using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    float brakeInput;
    float steeringInput;
    [SerializeField] float maxSteeringAngle = 30;
    [SerializeField] float motorPower = 3000.0f; // Adjust the value as needed
    [SerializeField] float brakePower = 2000.0f; // Adjust the value as needed
    [SerializeField] float gasDrag = 0.2f, idleDrag = 0.5f, brakeDrag = 1f, brakeThreshold = 2f;
    float slipAngle;
    float speed;
    public AnimationCurve steeringCurve;
    private Vector3 localVelocity;

    [SerializeField] GameController gc;

    void Update()
    {
        localVelocity = transform.InverseTransformDirection(rb.velocity);
        speed = rb.velocity.magnitude;
        ApplyWheelPos();
        CheckInput();
        ApplyMovement();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        //Debug.Log(brakeInput);
    }

    private void FixedUpdate()
    {
        if (Input.GetAxis("Vertical") != 0 && gc.PlayerFuel > 0)
        {
            gc.BurnFuel(gasInput);
        }
    }

    void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
    }

    void ApplyMovement()
    {
        if ((localVelocity.z > brakeThreshold && gasInput < 0f) || (localVelocity.z < -brakeThreshold && gasInput > 0)) //FRENAGEM
        {
            brakeInput = 1;
            rb.drag = brakeDrag;
        }
        else if (gasInput != 0) //ACELERACAO/RE
        {
            brakeInput = 0;
            rb.drag = gasDrag;
        }
        else if (steeringInput != 0)
        {
            brakeInput = 0.025f;
            rb.drag = gasDrag;
        }
        else
        {
            brakeInput = 0.05f;
            rb.drag = idleDrag;
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
        float steeringSensitivity = 2f; // Adjust the value as needed
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

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Damagable"))
            GameController.controller.Damage((rb.velocity.magnitude * 8f));

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
