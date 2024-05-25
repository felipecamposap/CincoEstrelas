using System.Collections;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    private GameObject player;
    [SerializeField] private float rotSpeed = 50f;

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        player = GameController.controller.player.gameObject;
    }

    void Update()
    {
        // Follow the player's position but keep the camera's height
        Vector3 posTarget = player.transform.position;
        posTarget = new Vector3(posTarget.x, transform.position.y, posTarget.z);
        transform.position = posTarget;

        // Follow the player's Y-axis rotation
        float targetYRotation = player.transform.eulerAngles.y;
        Quaternion targetRotation = Quaternion.Euler(transform.eulerAngles.x, targetYRotation, transform.eulerAngles.z);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);
    }
}
