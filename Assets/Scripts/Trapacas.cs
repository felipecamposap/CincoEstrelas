using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trapacas : MonoBehaviour
{
    [SerializeField] Toggle[] toggles;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(0.5f);
        toggles[0].isOn = GameController.controller.trapacas[0];
        toggles[1].isOn = GameController.controller.trapacas[1];
    }

    public void Indestrutivel(bool value)
    {
        GameController.controller.trapacas[0] = value;
    }

    public void GasolinaInfinita(bool value)
    {
        GameController.controller.trapacas[1] = value;
    }

}
