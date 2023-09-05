using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    [SerializeField] float gasInput;
    [SerializeField] float breakInput;
    [SerializeField] float steeringInput;

    [SerializeField] float motorPower;
    [SerializeField] float breakPower;
    float slipAngle;
    float speed;
    public AnimationCurve steeringCurve;

    void Update(){
        speed = rb.velocity.magnitude;
        ApplyWheelPos();
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBreak();
    }

    void CheckInput(){
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
        slipAngle = Vector3.SignedAngle(transform.forward, rb.velocity - transform.forward, Vector3.up);
        if (slipAngle < 120f){
            if (gasInput < 0){
                breakInput = Mathf.Abs(gasInput);
                gasInput = 0;
            }
        } else {
            breakInput = 0;
        }

        /*if (gasInput > 0){
            breakInput = 0;
            rb.drag = 0.2f;
        }
        else{
            rb.drag = 0.8f;
        }*/
    }

    void ApplyBreak(){
        colliders.FRWheel.brakeTorque = breakInput * breakPower * 0.7f;
        colliders.FLWheel.brakeTorque = breakInput * breakPower * 0.7f;
        colliders.BRWheel.brakeTorque = breakInput * breakPower * 0.3f;
        colliders.BLWheel.brakeTorque = breakInput * breakPower * 0.3f;
    }

    void ApplyMotor(){
        colliders.FRWheel.motorTorque = motorPower * gasInput;
        colliders.FLWheel.motorTorque = motorPower * gasInput;
    }

    void ApplySteering(){
        float steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    void ApplyWheelPos(){
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.BRWheel, wheelMeshes.BRWheel);
        UpdateWheel(colliders.BLWheel, wheelMeshes.BLWheel);
    }

    void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh){
        Quaternion rot;
        Vector3 pos;
        coll.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }

}
[System.Serializable]
public class WheelColliders{
    public WheelCollider FRWheel;
    public WheelCollider FLWheel;
    public WheelCollider BRWheel;
    public WheelCollider BLWheel;
}
[System.Serializable]
public class WheelMeshes{
    public MeshRenderer FRWheel;
    public MeshRenderer FLWheel;
    public MeshRenderer BRWheel;
    public MeshRenderer BLWheel;
}
