using UnityEngine;

public interface IObserverTrafficLight
{
    public void Notify();
}

public class TrafficLight : MonoBehaviour, IObserverTrafficLight
{
    public LinkedStreets[] streets;
    public bool[] isRed;
    [SerializeField] bool isTraffic;
    //[SerializeField] float trafficTime;
    [SerializeField] int index;

    private void OnEnable()
    {
        if (isTraffic)
        {
            GameController.controller.obsTrafficLight.AddListTrafficLight(this);
            //invokerepeating("changetraffic", traffictime, traffictime);
        }
        
    }

    //private void ChangeTraffic()
    //{
        
        
    //}

    public void Notify()
    {
        for (int i = 0; i < isRed.Length; i++)
        {
            isRed[i] = true;
        }
        if (index >= isRed.Length)
            index = 0;
        isRed[index] = false;
        index++;
    }
}
