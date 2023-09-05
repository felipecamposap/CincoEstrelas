using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class CameraMoviment : MonoBehaviour
{
    [SerializeField] private float posSpeed;
    [SerializeField] private float rotSpeed;
    [SerializeField] private Transform pos;
    [SerializeField] private Transform targetRot;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, pos.position, posSpeed * Time.deltaTime);
        //transform.LookAt(targetRot);
        transform.localRotation = Quaternion.Slerp(transform.rotation, targetRot.rotation, rotSpeed);
    }
}
