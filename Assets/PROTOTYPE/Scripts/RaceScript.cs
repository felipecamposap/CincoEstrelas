using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceScript : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] pos;
    [SerializeField] private int index = 0;

    [SerializeField] float volta;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 dir = (pos[index].transform.position - transform.position);
        if(dir.magnitude <= volta)
            index = (index + 1) % pos.Length;
        transform.rotation = Quaternion.Euler(0, -90 * index, 0);
        transform.position += dir.normalized * speed * Time.fixedDeltaTime;
    }
}
