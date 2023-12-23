using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlvoMinimapa : MonoBehaviour
{
    public int index;

    public void Start()
    {
        GameController.controller.alvoMinimapa = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(index == 2)
            index = 0;
        if(GameController.controller.minimapaAlvo[index] != null)
            transform.LookAt(GameController.controller.minimapaAlvo[index]);
    }
}
