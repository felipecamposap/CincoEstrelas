using UnityEngine;
using UnityEngine.UI;


public class HourPhoneController : MonoBehaviour
{
    [SerializeField] private Text hourText, minuteText;


    public void SetHour(int _hour, int _minute)
    {
        hourText.text = _hour.ToString("D2");
        minuteText.text = _minute.ToString("D2");
    }

}
