using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class PlayerMovement : MonoBehaviour
{
    private static readonly int EmissionColor = Shader.PropertyToID("_EmissionColor");
    [SerializeField] private Rigidbody rb;
    public WheelColliders colliders;
    public WheelMeshes wheelMeshes;
    public float gasInput;
    private float brakeInput, steeringInput, speed, initialCameraFOV, steeringAngle;
    public float motorPower = 5000.0f; // Adjust the value as needed
    [SerializeField] public float brakePower; // Adjust the value as needed
    private bool naCalcada;
    private int calcadas;

    [SerializeField]
    private float gasDrag = 0.005f, idleDrag = 0.5f, brakeDrag = 1.5f, brakeThreshold = 2f, fovTimer = 0f;

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
    private Camera mainCamera;
    public Transform[] carDoorPos;

    [SerializeField] private Animator animatorDoor;

    private void Start()
    {
        var materials = GetComponentInChildren<Renderer>().materials.ToList();
        breakLights = materials[5];
        GameController.controller.player = this;
        mainCamera = Camera.main;
        initialCameraFOV = mainCamera.fieldOfView;
    }

    private void Update()
    {
        speed = rb.linearVelocity.magnitude * 0.65f;
        GameController.controller.uiController.Velocity(speed / 30);
        if (!inGame)
        {
            brakeInput = rb.linearDamping * 30;
            ApplyBrake();
            ToggleVFX(tireSmokes, false);
            return;
        }

        CheckInput();
        ApplyMovement();
        ApplyMotor();
        ApplyBrake();
        ApplySteering();
    }

    private void FixedUpdate()
    {
        localVelocity = transform.InverseTransformDirection(rb.linearVelocity);
        if (inGame && Input.GetAxis("Vertical") != 0 && !GameController.controller.trapacas[1] &&
            GameController.controller.PlayerFuel > 0)
        {
            float multiplier = 1;
            if (rb.linearVelocity.magnitude is < 15 or > 30)
                multiplier += 1;
            GameController.controller.BurnFuel(gasInput * multiplier);
        }

        ApplyVFX();
        ApplyWheelPos();
    }

    private void ApplyVFX()
    {
        switch (gasInput)
        {
            case < 0 when localVelocity.z > brakeThreshold:
                SetBrakeLights(true);
                break;
            case < 0:
            case 0: //carro solto
                SetBrakeLights(false);
                ToggleVFX(tireSmokes, false);
                ToggleTrails(tireMarks, false);
                break;

            default: //acelerando
            {
                if (localVelocity.z < -brakeThreshold) // freando com o carro indo para frente
                {
                    SetBrakeLights(true);
                }
                else // acelerando com o carro indo para frente
                {
                    if (((localVelocity.z is > 0 and < 8) || localVelocity is { z: > 10, x: > 2 } ||
                         localVelocity is { z: < 8, x: > 3 }) &&
                        (colliders.BLWheel.isGrounded && colliders.BRWheel.isGrounded))
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

        if (localVelocity.z > 35)
        {
            if (!speedLines.isPlaying) speedLines.Play();
            ToggleVFX(wheelSpins, true);
            ToggleTrails(backlightTrails, true);
            ToggleTrails(edgeTrails, true);
            ApplyFOV(true);
        }
        else
        {
            ToggleVFX(wheelSpins, false);
            if (speedLines.isPlaying) speedLines.Stop();
            ToggleTrails(backlightTrails, false);
            ToggleTrails(edgeTrails, false);
            ApplyFOV(false);
        }
    }

    private void ApplyFOV(bool value)
    {
        if (value)
        {
            fovTimer += Time.fixedDeltaTime / 1.5f;
        }
        else
        {
            fovTimer -= Time.fixedDeltaTime;
        }

        fovTimer = Mathf.Clamp(fovTimer, 0, 1);
        mainCamera.fieldOfView = Mathf.SmoothStep(initialCameraFOV, 80, fovTimer);
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
                rb.linearDamping = brakeDrag;
                break;
            //dar ré
            case < 0:
                brakeInput = 0;
                rb.linearDamping = gasDrag/2;
                break;
            //carro solto
            case 0:
                rb.linearDamping = idleDrag;
                break;
            //acelerando
            default:
            {
                if (localVelocity.z < -brakeThreshold) // frear com o carro acelerando
                {
                    brakeInput = 1;
                    rb.linearDamping = brakeDrag;
                }
                else // acelerar
                {
                    brakeInput = 0;
                    rb.linearDamping = gasDrag;
                }

                break;
            }
        }
    }

    private void SetBrakeLights(bool value)
    {
        breakLights.SetColor(EmissionColor, (value ? Color.red : Color.black));
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

    private static void ToggleTrails(TrailRenderer[] trailRenderers, bool value)
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
        colliders.BRWheel.motorTorque =
            gasInput > 0 ? motorPower * gasInput : motorPower * gasInput / 1.75f; // Adjust the value as needed
        colliders.BLWheel.motorTorque =
            gasInput > 0 ? motorPower * gasInput : motorPower * gasInput / 1.75f; // Adjust the value as needed
    }

    private void ApplySteering()
    {
        steeringAngle = steeringInput * steeringCurve.Evaluate(speed);
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

    private static void UpdateWheel(WheelCollider coll, MeshRenderer wheelMesh)
    {
        coll.GetWorldPose(out var pos, out var rot);
        wheelMesh.transform.position = pos;
        wheelMesh.transform.rotation = rot;
    }

    private bool canDamage = true;

    public IEnumerator OnCollisionEnter(Collision collision)
    {
        Debug.Log(collision.gameObject.name);
        if (collision.gameObject.CompareTag("Cliente"))
        {
            GameController.controller.uiController.GameOver(2);
            Debug.Log("Atropelamento");
        }

        if (!inGame || !canDamage) yield break;
        if (collision.gameObject.CompareTag("Damagable") || collision.gameObject.CompareTag("Npc"))
        {
            canDamage = false;
            var damageValue = rb.linearVelocity.magnitude;
            yield return new WaitForSeconds(0.1f);
            damageValue -= rb.linearVelocity.magnitude;
            damageValue = Math.Max(1, damageValue);
            var contact = collision.contacts[0];
            Instantiate(danoFaisca, contact.point, Quaternion.identity);
            GameController.controller.penalty += 1;
            if (!GameController.controller.trapacas[0])
            {
                GameController.controller.Damage((damageValue * 5f));
                if (GameController.controller.carIntegrityCurrent < GameController.controller.carIntegrityMax / 4)
                {
                    ToggleVFX(damageVFX, true);
                }
            }
        }


        yield return new WaitForSeconds(0.25f);
        canDamage = true;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.CompareTag("Calcada") && calcadas == 0)
        {
            GameController.controller.penalty += 1;
            Debug.Log("Calcada");
        }

        if (col.gameObject.CompareTag("Calcada"))
            calcadas++;
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Calcada"))
            calcadas--;
    }


    public void UpgradeMotor()
    {
        motorPower += 200;
    }

    public void UpgradeBrake()
    {
        brakePower += 0.1f;
    }

    public void PlayerVictory()
    {
        if (GameController.controller.playerStar >= 10)
            vitoriaEfeito.SetActive(true);
        inGame = false;
    }

    public void OpenDoor(int value)
    {
        animatorDoor.SetInteger("State", value);
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