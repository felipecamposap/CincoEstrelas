using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObserverTrafficLight : MonoBehaviour
{
    [SerializeField] List<IObserverTrafficLight> m_Lights = new List<IObserverTrafficLight>();
    [SerializeField] float trafficTime;



    private void Start()
    {
        GameController.controller.obsTrafficLight = this;
        InvokeRepeating("CountTime", trafficTime, trafficTime);
    }

    public void AddListTrafficLight(IObserverTrafficLight _m_Lights)
    {
        m_Lights.Add(_m_Lights);
    }

    public void NotityTrafficLights()
    {
        foreach (IObserverTrafficLight _m_Light in m_Lights)
        {
            _m_Light?.Notify();
        }
    }

    public void CountTime()
    {
        NotityTrafficLights();
    }
}
