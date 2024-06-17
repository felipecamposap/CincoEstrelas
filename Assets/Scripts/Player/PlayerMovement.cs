using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    private float brakeInput;
    private float steeringInput;
    public float motorPower = 5000.0f; // Adjust the value as needed
    [SerializeField] private float brakePower = 10000.0f; // Adjust the value as needed
    [SerializeField] private float gasDrag = 0.005f, idleDrag = 0.5f, brakeDrag = 1.5f, brakeThreshold = 2f;
    private float regularStiffness;
    private float speed;
    public AnimationCurve steeringCurve;
    private Vector3 localVelocity;
    [SerializeField] private GameObject danoFaisca, luzesFreio;
    [SerializeField] private GameObject vitoriaEfeito;
    public bool inGame = true;
    private Material breakLights;
    public ParticleSystem[] damageVFX;
    [SerializeField] private ParticleSystem[] wheelSpins, tireSmokes;
    [SerializeField] private TrailRenderer[] tireMarks, backlightTrails, edgeTrails;
    [SerializeField] private ParticleSystem speedLines;

    private void Start()
    {
        var materials = GetComponentInChildren<Renderer>().materials.ToList();
        breakLights = materials[5];
        Debug.Log(breakLights.name);
        GameController.controller.player = this;
        inGame = true;
    }

    private void Update()
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
        //Debug.Log(brakeInput);
    }

    private void FixedUpdate()
    {
        if (inGame && Input.GetAxis("Vertical") != 0 && !GameController.controller.trapacas[1] &&
            GameController.controller.PlayerFuel > 0)
        {
            GameController.controller.BurnFuel(gasInput);
        }

        ApplyVFX();
        ApplyWheelPos();
    }

    private void ApplyVFX()
    {
        Debug.Log(localVelocity);
        switch (gasInput)
        {
            case < 0 when localVelocity.z > brakeThreshold:
                SetBrakeLights(true);
                break;
            case < 0:
            case 0: //carro solto
                SetBrakeLights(false);
                break;

            default: //acelerando
            {
                if (localVelocity.z < -brakeThreshold) // freando com o carro indo para frente
                {
                    SetBrakeLights(true);
                }
                else // acelerando com o carro indo para frente
                {
                    if ((localVelocity.z is > 0 and < 8) || localVelocity is { z: > 10, x: > 2 } || localVelocity is { z: < 8, x: > 3 })
                    {
                        ToggleVFX(tireSmokes, true);
                        ToggleTrails(tireMarks, true);
                    }
                    else
                    {
                        ToggleVFX(tireSmokes, false);
                        ToggleTrails(tireMarks, false);
                    }

                    SetBrakeLights(false);
                }


                break;
            }
        }

        if (localVelocity.z > 30)
        {
            if (!speedLines.isPlaying) speedLines.Play();
            ToggleVFX(wheelSpins, true);
            ToggleTrails(backlightTrails, true);
            ToggleTrails(edgeTrails, true);
        }
        else
        {
            ToggleVFX(wheelSpins, false);
            if (speedLines.isPlaying) speedLines.Stop();
            ToggleTrails(backlightTrails, false);
            ToggleTrails(edgeTrails, false);
        }
    }

    private void CheckInput()
    {
        gasInput = Input.GetAxis("Vertical");
        steeringInput = Input.GetAxis("Horizontal");
    }

    private void ApplyMovement()
    {
        switch (gasInput)
        {
            case < 0 when localVelocity.z > brakeThreshold:
                brakeInput = 1;
                rb.drag = brakeDrag;
                break;
            //dar ré
            case < 0:
                brakeInput = 0;
                rb.drag = gasDrag;
                break;
            //carro solto
            case 0:
                rb.drag = idleDrag;
                break;
            //acelerando
            default:
            {
                if (localVelocity.z < -brakeThreshold) // frear com o carro acelerando
                {
                    //Debug.Log("Frear");
                    brakeInput = 1;
                    rb.drag = brakeDrag;
                }
                else // acelerar
                {
                    brakeInput = 0;
                    rb.drag = gasDrag;
                }

                break;
            }
        }
    }

    private void SetBrakeLights(bool value)
    {
        breakLights.SetColor("_EmissionColor", (value ? Color.red : Color.black));
    }

    public void ToggleVFX(ParticleSystem[] particleSystems, bool value)
    {
        if (value)
        {
            foreach (var ps in particleSystems)
            {
                if (!ps.isPlaying) ps.Play();
            }
        }
        else
        {
            foreach (var ps in particleSystems)
            {
                if (ps.isPlaying) ps.Stop();
            }
        }
    }

    public void ToggleTrails(TrailRenderer[] trailRenderers, bool value)
    {
        if (value)
        {
            foreach (var tr in trailRenderers)
            {
                if (!tr.emitting) tr.emitting = true;
            }
        }
        else
        {
            foreach (var tr in trailRenderers)
            {
                if (tr.emitting) tr.emitting = false;
            }
        }
    }

    private void ApplyBrake()
    {
        colliders.FRWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.FLWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.BRWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
        colliders.BLWheel.brakeTorque = brakeInput * brakePower; // Adjust the value as needed
    }

    private void ApplyMotor()
    {
        colliders.BRWheel.motorTorque = motorPower * gasInput; // Adjust the value as needed
        colliders.BLWheel.motorTorque = motorPower * gasInput; // Adjust the value as needed
    }

    private void ApplySteering()
    {
        var steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
        colliders.FRWheel.steerAngle = steeringAngle;
        colliders.FLWheel.steerAngle = steeringAngle;
    }

    private void ApplyWheelPos()
    {
        UpdateWheel(colliders.FRWheel, wheelMeshes.FRWheel);
        UpdateWheel(colliders.FLWheel, wheelMeshes.FLWheel);
        UpdateWheel(colliders.BRWheel, wheelMeshes.BRWheel);
        UpdateWheel(colliders.BLWheel, wheelMeshes.BLWheel);
    }

    private void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        Quaternion rot;
        Vector3 pos;
        coll.GetWorldPose(out pos, out rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        if (inGame)
        {
            var damageValue = rb.velocity.magnitude * 1.5f;
            yield return new WaitForSeconds(0.1f);
            damageValue -= rb.velocity.magnitude;
            if (collision.gameObject.CompareTag("Damagable") || collision.gameObject.CompareTag("Npc"))
            {
                var contact = collision.contacts[0];
                Instantiate(danoFaisca, contact.point, Quaternion.identity);
                GameController.controller.penalty += 1;
                if (!GameController.controller.trapacas[0])
                {
                    GameController.controller.Damage((damageValue * 10f));
                    if (GameController.controller.carIntegrityCurrent < GameController.controller.carIntegrityMax / 4)
                    {
                        ToggleVFX(damageVFX, true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Calcada"))
        {
            GameController.controller.penalty += 1;
        }
    }

    public void UpgradeMotor()
    {
        motorPower += 200;
    }

    public void PlayerVictory()
    {
        if (GameController.controller.playerStar >= 10)
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