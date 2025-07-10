using UnityEngine;
using UnityEngine.UI;


public class PreGameCine : MonoBehaviour
{
    [SerializeField] private Text dayBill;
    [SerializeField] GameObject[] canvasActivation;


    // Start is called before the first frame update
    private void Start()
    {
        for(int i = 0; i < canvasActivation.Length; i++)
            canvasActivation[i].SetActive(true);
        gameObject.SetActive(true);
        dayBill.text = $"R$: {GameController.controller.GetDailyBill():F2}";
    }

    public void StartGame()
    {
        GameController.controller.player.inGame = true;
        this.gameObject.SetActive(false);
    }
}
