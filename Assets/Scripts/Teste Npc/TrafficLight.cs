using UnityEngine;


public interface IObserverTrafficLight
{
    public void Notify();
}

[DefaultExecutionOrder(99)]
public class TrafficLight : MonoBehaviour, IObserverTrafficLight
{
    public LinkedStreets[] streets;
    public bool[] isRed;
    [SerializeField] private bool isTraffic;
    [SerializeField] private int index;
    [SerializeField] Renderer[] meshs;
    [SerializeField] Database dataBase;

    private void OnEnable()
    {
        if (isTraffic)
        {
            GameController.controller.obsTrafficLight.AddListTrafficLight(this);
        }
        
    }

    public void Notify()
    {
        Material[] currentMaterials = meshs[0].materials;
        currentMaterials[2] = dataBase.lightMaterials[0];

        for (var i = 0; i < isRed.Length; i++)
        {
            isRed[i] = true;
            meshs[i].materials = currentMaterials;
            
        }
        if (index >= isRed.Length)
            index = 0;
        isRed[index] = false;
        currentMaterials[2] = dataBase.lightMaterials[2];
        meshs[index].materials = currentMaterials;
        index++;
    }
}
