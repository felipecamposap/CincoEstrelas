using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f, lastSensitivity; // Adjust the sensitivity of the mouse movement
    public float maxPitch = 30.0f; // Maximum pitch angle in degrees
    private GameObject player;
    private Camera mainCamera;
    private bool locked, reverse;
    [SerializeField] private float camSpeed;
    private Quaternion lookTarget;
    [SerializeField] private float rotSpeed;
    private float idleCamTimer = 0;
    [SerializeField] private float maxIdleCamTimer = 1f;
    private PlayerMovement playerMovement;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        lastSensitivity = sensitivity;
        Application.targetFrameRate = 999;
        mainCamera = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameController.controller.player.gameObject;
        playerMovement = player.GetComponent<PlayerMovement>();
        lookTarget = player.transform.rotation;
    }

    private void Update()
    {
        gameObject.transform.position = player.transform.position;
        //transform.position = Vector3.Lerp(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), pos.position, camSpeed * Time.deltaTime);  

        // if (Input.GetKeyDown(KeyCode.Mouse1))
        // {
        //     mainCamera.fieldOfView = 25;
        // }
        //
        // if (Input.GetKeyUp(KeyCode.Mouse1))
        // {
        //     mainCamera.fieldOfView = 60;
        // }


        var cellPhoneLift = GameController.controller.uiController.CellPhoneLift();

        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            if (GameController.controller.isInteracting) { return; }
            GameController.controller.uiController.CellPhoneAnimation(cellPhoneLift ? 1 : 0);
        }

        // Get the mouse input
        var mouseX = Input.GetAxis("Mouse X");
        var mouseY = Input.GetAxis("Mouse Y");

        if ((mouseX != 0f || mouseY != 0f) && !cellPhoneLift)
        {
            idleCamTimer = 0;
            LockCam(false, mouseX, mouseY);
        }
        else if (playerMovement.gasInput != 0)
            idleCamTimer += Time.deltaTime;
        else
            idleCamTimer = 0;

        if (idleCamTimer >= maxIdleCamTimer || cellPhoneLift )
        {
            LockCam(true, mouseX, mouseY);
        }

        if(!player.GetComponent<PlayerMovement>().inGame)
        {
            LockCam(true, mouseX, mouseY);
            //lookTarget = GameController.controller.minimapaAlvo[0].transform.rotation;
            lookTarget = Quaternion.LookRotation(GameController.controller.minimapaAlvo[0].transform.position - player.transform.position);
            //lookTarget.x = -lookTarget.x;
            
        }
        else
        {
            lookTarget = player.transform.rotation;
        }

        if (locked)
        {
            
            lookTarget = new Quaternion(0f, -lookTarget.y, 0f, -lookTarget.w); // evitar que a camera automatica rotacione em eixos errados
            transform.rotation = Quaternion.Slerp(transform.rotation, lookTarget, rotSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up * (mouseX * sensitivity * Time.deltaTime));
            var newPitch = transform.eulerAngles.x - (mouseY * sensitivity * Time.deltaTime);
            var clampedPitch = ClampAngle(newPitch, -10f, 40f);
            transform.rotation = Quaternion.Euler(clampedPitch, transform.eulerAngles.y, 0f);
        }
        
    }

    private static float ClampAngle(float angle, float min, float max)
    {
        if (angle is < 90 or > 270)
        {       // if angle in the critic region...
            if (angle > 180) angle -= 360;  // convert all angles to -180..+180
            if (max > 180) max -= 360;
            if (min > 180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if (angle < 0) angle += 360;  // if angle negative, convert to 0..360
        return angle;
    }


    private void LockCam(bool locked, float mouseX, float mouseY)
    {
        this.locked = locked;
        sensitivity = locked ? 0f : lastSensitivity;
    }
}
