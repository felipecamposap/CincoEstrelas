using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private float posSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private Transform pos;
    [SerializeField] private Transform targetRot;
    private float posY;

    void Start()
    {
        posY = transform.position.y;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(new Vector3(transform.position.x, posY, transform.position.z), pos.position, posSpeed * Time.deltaTime);


        //transform.LookAt(targetRot);
        transform.localRotation = Quaternion.Slerp(new Quaternion(0f, transform.rotation.y, 0f, transform.rotation.w), targetRot.rotation, rotSpeed);
        
    }
}
