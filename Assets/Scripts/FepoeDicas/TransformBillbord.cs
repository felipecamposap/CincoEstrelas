using UnityEngine;


public class TransformBillbord : MonoBehaviour
{
    [SerializeField] private bool keepDistance;
    [SerializeField] private float distance;
    private Transform cam;
    [SerializeField] private Vector3 offset;
    private Vector3 originalPos;


    // Start is called before the first frame update
    private void Start()
    {
        originalPos = transform.position;
        cam = Camera.main.transform;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
        transform.LookAt(cam.position, Vector3.up);
        transform.rotation *= Quaternion.Euler(offset);
        if(keepDistance)
        {
            transform.position = new Vector3(originalPos.x, originalPos.y, cam.position.z + distance);
            //float _distance = Mathf.Abs(cam.position.z - transform.position.z);
            //float diference = distance - _distance;
            //if (_distance >= 5)
            //    transform.position = originalPos + Vector3.forward * (diference / 2);
            //Debug.Log(diference);
        }

    }

}
