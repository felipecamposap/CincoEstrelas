using UnityEngine;
using UnityEngine.UI;

public class ResetInteractableOnEnabled : MonoBehaviour
{
    [SerializeField] private Slider[] slider;


    private void OnEnable()
    {
        foreach(var slider in slider)
        {
            slider.value = 0;
        }
    }
}
