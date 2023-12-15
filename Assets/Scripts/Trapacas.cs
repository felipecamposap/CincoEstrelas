using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trapacas : MonoBehaviour
{
    public void Indestrutivel(bool value)
    {
        GameController.controller.trapacas[0] = value;
    }

    public void GasolinaInfinita(bool value)
    {
        GameController.controller.trapacas[0] = value;
    }

}
