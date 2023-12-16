using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableEvent : MonoBehaviour
{
    [SerializeField] GameObject[] objects;


    private void OnDisable()
    {
        for(int i = 0; i < objects.Length; i++)
        {
            objects[i].SetActive(false);
        }
    }
}
