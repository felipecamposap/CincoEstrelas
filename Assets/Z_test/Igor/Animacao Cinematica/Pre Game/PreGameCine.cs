using UnityEngine;
using UnityEngine.UI;


public class PreGameCine : MonoBehaviour
{
    [SerializeField] Text dayBill;


    // Start is called before the first frame update
    void Start()
    {
        dayBill.text = $"R$: {GameController.controller.GetDailyBill():F2}";
    }

    public void StartGame()
    {
        GameController.controller.player.inGame = true;
        this.gameObject.SetActive(false);
    }
}
