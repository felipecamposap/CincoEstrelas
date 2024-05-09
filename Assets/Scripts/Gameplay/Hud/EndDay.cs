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
        animator.SetBool("FadeOut", false);
        moneyCutscene = 0;
        dayBill.text = $"Custo diário: {GameController.controller.GetDailyBill() / 30:F2}";
        InvokeRepeating("MoneyCutscene", 0, 0.01f);
    }

    public void MoneyCutscene()
    {
        moneyCutscene += 0.1f;
        if(moneyCutscene > GameController.controller.PlayerMoney)
        {
            moneyCutscene = GameController.controller.PlayerMoney;
            CancelInvoke();
            animator.SetBool("FadeOut", true);
        }
        currentMoney.text = $"Ganho do dia: {moneyCutscene:F2}";
    }

    public void Close() 
    {
        this.gameObject.SetActive(false);
        GameController.controller.NextDay();
    }

}
