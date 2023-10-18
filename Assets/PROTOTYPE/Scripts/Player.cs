using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    float brakeInput;
    float steeringInput;
    float maxSteeringAngle = 60;

    [SerializeField] float motorPower = 3000.0f; // Adjust the value as needed
    [SerializeField] float brakePower = 2000.0f; // Adjust the value as needed
    float slipAngle;
    float speed;
    public AnimationCurve steeringCurve;

    [SerializeField] GameController gc;

    void Update()
    {
        speed = rb.velocity.magnitude;
        ApplyWheelPos();
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
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
        slipAngle = Vector3.SignedAngle(transform.forward, rb.velocity - transform.forward, Vector3.up);
        if (slipAngle < 120f)
        {
            if (gasInput < 0)
            {
                brakeInput = -gasInput;
            }
        }
        else
        {
            brakeInput = 0;
        }

        if (gasInput > 0)
        {
            rb.drag = 0.1f; // Adjust the value as needed
        }
        else
        {
            rb.drag = 1f; // Adjust the value as needed
        }
    }

    void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower * 0.6f; // Adjust the value as needed
        colliders.FLWheel.brakeTorque = brakeInput * brakePower * 0.6f; // Adjust the value as needed
        colliders.BRWheel.brakeTorque = brakeInput * brakePower * 0.4f; // Adjust the value as needed
        colliders.BLWheel.brakeTorque = brakeInput * brakePower * 0.4f; // Adjust the value as needed
    }

    void ApplyMotor()
    {
        colliders.FRWheel.motorTorque = motorPower * gasInput;
        colliders.FLWheel.motorTorque = motorPower * gasInput;
        colliders.BRWheel.motorTorque = motorPower * gasInput * 0.9f; // Adjust the value as needed
        colliders.BLWheel.motorTorque = motorPower * gasInput * 0.9f; // Adjust the value as needed
    }

    void ApplySteering()
    {
        float steeringSensitivity = 2.0f; // Adjust the value as needed
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
<<<<<<< Updated upstream
=======

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Damagable"))
            GameController.controller.Damage((rb.velocity.magnitude * 8f));
    }

>>>>>>> Stashed changes
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
