using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float rotationSpeed = 2.0f;
    public float jumpForce = 5.0f;
    public float gravity = 20.0f;
    public float pitchSpeed = 2.0f; // Adjust pitch (looking up and down) speed

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    private float pitch = 0.0f;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();

        // Hide the mouse cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        // Check if the character is grounded
        if (characterController.isGrounded)
        {
            // Calculate movement direction based on input
            var horizontalInput = Input.GetAxis("Horizontal");
            var verticalInput = Input.GetAxis("Vertical");
            var forwardMovement = transform.TransformDirection(Vector3.forward) * verticalInput * moveSpeed;
            var strafeMovement = transform.TransformDirection(Vector3.right) * horizontalInput * moveSpeed;
            moveDirection = forwardMovement + strafeMovement;

            // Jump
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Rotate the player based on mouse input
        var mouseX = Input.GetAxis("Mouse X") * rotationSpeed;
        var mouseY = Input.GetAxis("Mouse Y") * pitchSpeed;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -90.0f, 90.0f);

        // Apply the rotations
        transform.Rotate(Vector3.up * mouseX);
        Camera.main.transform.localRotation = Quaternion.Euler(pitch, 0, 0);

        // Move the character controller
        characterController.Move(moveDirection * Time.deltaTime);
    }
}