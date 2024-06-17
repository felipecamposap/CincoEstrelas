using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvent : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;


    private void OnDisable()
    {
        for(var i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
