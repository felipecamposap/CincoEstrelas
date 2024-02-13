using UnityEngine;
using UnityEngine.UI;

public class ResetInteractableOnEnabled : MonoBehaviour
{
    [SerializeField] Slider[] slider;


    void OnEnable()
    {
        foreach(Slider slider in slider)
        {
            slider.value = 0;
        }
    }
}
