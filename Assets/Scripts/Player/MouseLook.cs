using Unity.Mathematics;
using UnityEngine;


public class MouseLook : MonoBehaviour
{
    public float sensitivity = 8.0f, lastSensitivity; // Adjust the sensitivity of the mouse movement
    public float maxPitch = 30.0f; // Maximum pitch angle in degrees
    public GameObject player;
    private Camera cameraMain;
    private quaternion initialRotation;
    private bool locked;
    [SerializeField] private Transform pos;
    [SerializeField] private float camSpeed;
    [SerializeField] private Transform lookRot;
    [SerializeField] private float rotSpeed;

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

        if (Input.GetKeyDown(KeyCode.E) && Time.timeScale > 0)
        {
            bool cellPhoneLift = GameController.controller.uiController.CellPhoneLift();
            LockCam(!cellPhoneLift);
            GameController.controller.uiController.CellPhoneAnimation(cellPhoneLift ? 1 : 0);
        }
    }

    void FixedUpdate()
    {
        // Get the mouse input
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // Rotate the camera around the Y-axis based on mouse X input
        transform.Rotate(Vector3.up * mouseX * sensitivity);

        // Calculate the new pitch rotation based on mouse Y input
        float newPitch = transform.eulerAngles.x - (mouseY * sensitivity);

        // Clamp the pitch angle between 0 and maxPitch
        float clampedPitch = Mathf.Clamp(newPitch, 0f, maxPitch);

        if (locked)
        {
            //transform.rotation = Quaternion.Euler(player.transform.rotation.x   , player.transform.rotation.y, 0f);
            transform.localRotation = Quaternion.Slerp(new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), lookRot.rotation, rotSpeed * Time.deltaTime);
        }
        else
        {
            // Apply the new rotation with the clamped pitch
            transform.rotation = Quaternion.Euler(clampedPitch, transform.eulerAngles.y, 0f);
        }
    }

    public void LockCam(bool locked)
    {
        if (locked)
        {
            this.locked = locked;
            sensitivity = 0f;
            //transform.rotation = Quaternion.Euler(player.transform.rotation.x, 0f, 0f);
        }
        else
        {
            this.locked = false;
            transform.parent = null;
            sensitivity = lastSensitivity;
        }

    }
}
