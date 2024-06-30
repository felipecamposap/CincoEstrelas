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
    private int isCellphoneLift;


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
        if(other.CompareTag("Player") && !GameController.controller.minimapaAlvo[0]){
            thisCanvas.SetActive(true);
            playerTouch = true;
            GameController.controller.isInteracting = true;
            isCellphoneLift = GameController.controller.uiController.CellPhoneLift() ? 0 : 1;
            if(isCellphoneLift == 0)
                GameController.controller.uiController.CellPhoneAnimation(1);

        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !GameController.controller.minimapaAlvo[0])
        {
            thisCanvas.SetActive(false);
            playerTouch = false;
            GameController.controller.isInteracting = false;
            if(isCellphoneLift == 0)
                GameController.controller.uiController.CellPhoneAnimation(isCellphoneLift);
        }
    }

}
