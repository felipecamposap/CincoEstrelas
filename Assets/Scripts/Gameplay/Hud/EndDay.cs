using UnityEngine;
using UnityEngine.UI;


public class EndDay : MonoBehaviour
{
    [SerializeField] Text dayBill;
    [SerializeField] Text currentMoney;
    float moneyCutscene;
    [SerializeField] Animator animator;

    public void OnEnable()
    {
        dayBill.text = $"Custo diário: R${GameController.controller.GetDailyBill():F2}";
        moneyCutscene = 0;
        animator.SetBool("FadeOut", false);
        InvokeRepeating("MoneyCutscene", 2, 0.01f);
    }

    public void MoneyCutscene()
    {
        //Debug.Log(GameController.controller.GetDailyBill() + GameController.controller.debtDay);
        moneyCutscene += (GameController.controller.GetDailyBill() + GameController.controller.debtDay) * 0.01f;
        //moneyCutscene += 0.01f;
        if (moneyCutscene > GameController.controller.PlayerMoney)
        {
            moneyCutscene = GameController.controller.PlayerMoney;
            CancelInvoke();
            animator.SetBool("FadeOut", true);
        }
        currentMoney.text = $"Ganho do dia: R${moneyCutscene:F2}";
    }

    public void Close() 
    {
        this.gameObject.SetActive(false);
        GameController.controller.NextDay();
    }

}
