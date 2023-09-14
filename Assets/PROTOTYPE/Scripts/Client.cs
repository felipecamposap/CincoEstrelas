using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{
    [SerializeField] private int touchPlayer;
    [SerializeField] private Transform target;
    [SerializeField] private float speed;
    [SerializeField] private Collider coll;
    private GameController gc;

    private void Awake()
    {
        gc = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        if (touchPlayer == 1 || touchPlayer == 3){
            transform.position = Vector3.Lerp(transform.position, target.position, speed * Time.deltaTime);
            if(Vector3.Distance(transform.position, target.position) <= 1){
                if (target.CompareTag("Player"))
                {
                    touchPlayer++;
                    transform.position = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
                    transform.rotation = target.transform.localRotation;
                    //transform.localRotation = new Quaternion(1, transform.localRotation.y, transform.localRotation.z, 1);
                    transform.parent = target.transform;
                }else{
                    Destroy(gameObject);
                    Destroy(target.gameObject);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (touchPlayer == 0 && other.CompareTag("Player")){
            coll.enabled = false;
            target = other.transform;
            touchPlayer++;
            
        }else if (touchPlayer == 2 && other.CompareTag("Destination")){
            gc.GetPaid(5f);
            transform.parent = null;
            target = other.transform;
            touchPlayer++;
        }
    }
}
