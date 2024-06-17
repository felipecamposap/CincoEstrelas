using System.Collections.Generic;
using UnityEngine;


[DefaultExecutionOrder(0)]
public class ObserverTrafficLight : MonoBehaviour
{
    [SerializeField] private List<IObserverTrafficLight> m_Lights = new List<IObserverTrafficLight>();
    [SerializeField] private float trafficTime;



    private void Awake()
    {
        GameController.controller.obsTrafficLight = this;
        InvokeRepeating(nameof(CountTime), trafficTime, trafficTime);
    }

    public void AddListTrafficLight(IObserverTrafficLight _m_Lights)
    {
        m_Lights.Add(_m_Lights);
    }

    private void NotityTrafficLights()
    {
        foreach (var _m_Light in m_Lights)
        {
            _m_Light?.Notify();
        }
    }

    public void CountTime()
    {
        NotityTrafficLights();
    }
}
