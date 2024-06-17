using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

public class RaceScript : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private Transform[] pos;
    [SerializeField] private GameObject[] wheels;
    [SerializeField] private int index = 0;
    private NavMeshAgent nva;

    private void Start()
    {
        rb.GetComponent<Rigidbody>();
        nva = GetComponentInParent<NavMeshAgent>();
        nva.SetDestination(pos[index].position);
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.CompareTag("Waypoint")) 
        {
            index = (index + 1) % pos.Length;
            nva.SetDestination(pos[index].position);
        }
    }

    private void OnCollisionEnter(Collision col)
    {
        if(col.collider.gameObject.CompareTag("Player") || col.collider.gameObject.CompareTag("NPC"))
        {
            nva.isStopped = true;
            rb.freezeRotation = false;
            Invoke("Resume", 5f);
        }
    }

    private void Resume()
    {
        var rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        nva.isStopped = false;
        rb.freezeRotation = true;
    }
}