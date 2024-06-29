using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider[] mixerSliders;
    [SerializeField] private bool startPlay;


    public void Start()
    {
        //int audioValue;
        //int sliderValue;

        if (SceneManager.GetActiveScene().buildIndex == 0 && startPlay)
        {
            // ----- Declarar volume Geral ----- \\
            mixerSliders[0].value = 3;
            audioMixer.SetFloat("Master", -10);

            // ----- Declarar volume Musica ----- \\
            mixerSliders[1].value = 3;
            audioMixer.SetFloat("Musica", -10);

            // ----- Declarar volume Efeito ----- \\
            mixerSliders[2].value = 3;
            audioMixer.SetFloat("Efeitos", -10);

            startPlay = false;
            this.gameObject.SetActive(false);
        } 
        else
        {
            float audioValue;
            int sliderValue;
            // ----- Declarar volume Geral ----- \\
            CheckPrefs("Master", out audioValue, out sliderValue);
            mixerSliders[0].value = sliderValue;
            audioMixer.SetFloat("Master", audioValue);

            // ----- Declarar volume Musica ----- \\
            CheckPrefs("Musica", out audioValue, out sliderValue);
            mixerSliders[1].value = sliderValue;
            audioMixer.SetFloat("Musica", audioValue);

            // ----- Declarar volume Efeito ----- \\
            CheckPrefs("Efeitos", out audioValue, out sliderValue);
            mixerSliders[2].value = sliderValue;
            audioMixer.SetFloat("Efeitos", audioValue);
        }

    }

    private void CheckPrefs(string _name, out float audioValue, out int sliderValue)
    {
        sliderValue = 0;
        audioMixer.GetFloat(_name, out audioValue);
        Debug.Log(_name + " : " + audioValue);//+ " - " + PlayerPrefs.GetInt(_name));

        //if (PlayerPrefs.HasKey(_name))
        //{
        //    Debug.Log("Find: " + _name);
        //    audioValue = PlayerPrefs.GetInt(_name);
            switch (audioValue)
            {
                case -88:
                    sliderValue = 0;
                    break;

                case -40:
                    sliderValue = 1;
                    break;

                case -20:
                    sliderValue = 2;
                    break;

                case -10:
                    sliderValue = 3;
                    break;

                case 5:
                    sliderValue = 4;
                    break;
            }
        //}
        //else
        //{
        //    audioValue = -20;
        //    sliderValue = 2;
        //}
    }

    public void MasterVolume()
    {
        var value = 0;
        switch (mixerSliders[0].value)
        {
            case 0:
                value = -88;
                break;

            case 1:
                value = -40;
                break;

            case 2:
                value = -20;
                break;

            case 3:
                value = -10;
                break;

            case 4:
                value = 5;
                break;
        }

        audioMixer.SetFloat("Master", value);
    }

    public void MusicaVolume()
    {
        var value = 0;
        switch (mixerSliders[1].value)
        {
            case 0:
                value = -88;
                break;

            case 1:
                value = -40;
                break;

            case 2:
                value = -20;
                break;

            case 3:
                value = -10;
                break;

            case 4:
                value = 5;
                break;
        }

        audioMixer.SetFloat("Musica", value);
    }

    public void EfeitoVolume()
    {
        var value = 0;
        switch (mixerSliders[2].value)
        {
            case 0:
                value = -88;
                break;

            case 1:
                value = -40;
                break;

            case 2:
                value = -20;
                break;

            case 3:
                value = -10;
                break;

            case 4:
                value = 5;
                break;
        }

        audioMixer.SetFloat("Efeitos", value);
    }

}
