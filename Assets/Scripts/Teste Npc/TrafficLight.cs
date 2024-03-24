using UnityEngine;

public class TrafficLight : MonoBehaviour
{
    public LinkedStreets[] streets;
    public bool[] isRed;
    [SerializeField] bool isTraffic;
    [SerializeField] float trafficTime;
    [SerializeField] int index;

    private void Start()
    {
        if(isTraffic)
        {
            InvokeRepeating("ChangeTraffic", trafficTime, trafficTime);
        }
    }

    private void ChangeTraffic()
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
