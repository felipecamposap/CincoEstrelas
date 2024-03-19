using Unity.Mathematics;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    public float sensitivity = 100f, lastSensitivity; // Adjust the sensitivity of the mouse movement
    public float maxPitch = 30.0f; // Maximum pitch angle in degrees
    private GameObject player;
    private Camera cameraMain;
    private quaternion initialRotation;
    private bool locked;
    [SerializeField] private float camSpeed;
    [SerializeField] private Transform lookRot;
    [SerializeField] private float rotSpeed;
    float idleCamTimer = 0;
    [SerializeField] float maxIdleCamTimer = 1f;

    void Start()
    {
        initialRotation = transform.rotation;
        lastSensitivity = sensitivity;
        Application.targetFrameRate = 999;
        cameraMain = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        gameObject.transform.position = player.transform.position;
        //transform.position = Vector3.Lerp(new Vector3(transform.position.x, player.transform.position.y, transform.position.z), pos.position, camSpeed * Time.deltaTime);  

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            cameraMain.fieldOfView = 25;
        }

        if (Input.GetKeyUp(KeyCode.Mouse1))
        {
            cameraMain.fieldOfView = 60;
        }


        bool cellPhoneLift = GameController.controller.uiController.CellPhoneLift();

        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            GameController.controller.uiController.CellPhoneAnimation(cellPhoneLift ? 1 : 0);
        }

        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        if ((mouseX != 0f || mouseY != 0f) && !cellPhoneLift)
        {
            idleCamTimer = 0;
            LockCam(false, mouseX, mouseY);
        }
        else
        {
            idleCamTimer += Time.deltaTime;
        }

        if (idleCamTimer >= maxIdleCamTimer || cellPhoneLift)
        {
            LockCam(true, mouseX, mouseY);
        }


        if (locked)
        {
            //transform.rotation = Quaternion.Euler(player.transform.rotation.x   , player.transform.rotation.y, 0f);
            Quaternion correctedRot = lookRot.rotation;
            correctedRot.z = 0;
            correctedRot.x = 0;
            transform.rotation = Quaternion.Slerp(transform.rotation, correctedRot, rotSpeed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(Vector3.up * mouseX * sensitivity * Time.deltaTime);
            float newPitch = transform.eulerAngles.x - (mouseY * sensitivity * Time.deltaTime);
            float clampedPitch = ClampAngle(newPitch, -10f, 40f);
            transform.rotation = Quaternion.Euler(clampedPitch, transform.eulerAngles.y, 0f);
        }
    }

    float ClampAngle(float angle, float min, float max)
    {
        if (angle < 90 || angle > 270)
        {       // if angle in the critic region...
            if (angle > 180) angle -= 360;  // convert all angles to -180..+180
            if (max > 180) max -= 360;
            if (min > 180) min -= 360;
        }
        angle = Mathf.Clamp(angle, min, max);
        if (angle < 0) angle += 360;  // if angle negative, convert to 0..360
        return angle;
    }



    public void LockCam(bool locked, float mouseX, float mouseY)
    {
        if (locked)
        {
            this.locked = locked;
            sensitivity = 0f;
        }
        else
        {
            this.locked = false;
            sensitivity = lastSensitivity;
        }

    }
}
