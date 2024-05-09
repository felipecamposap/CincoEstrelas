using UnityEngine;
using UnityEngine.UI;


public class InterfaceInterativos : MonoBehaviour
{
    [SerializeField] private GameObject thisCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private Color[] colorVar;
    [SerializeField] private Image imgSlider;
    [SerializeField] private bool playerTouch = false;
    [Header("Coloque o gameObject da interface da contru��o: ")]
    [SerializeField] private GameObject uiObject;


    private void Update()
    {
        if (playerTouch)
            if (Input.GetKey(KeyCode.E) && thisCanvas){
                slider.value += Time.deltaTime;
                imgSlider.color = Color.Lerp(colorVar[0], colorVar[1], slider.value);
                if (slider.value >= 1) { 
                    //thisCanvas.SetActive(false);
                    uiObject.SetActive(true);
                }
            }else
                slider.value = 0;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player")){
            thisCanvas.SetActive(true);
            playerTouch = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")){
            thisCanvas.SetActive(false);
            playerTouch = false;
        }
    }

}
