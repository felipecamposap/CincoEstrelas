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
            //CheckPrefs("MasterVolume", out audioValue, out sliderValue);
            //mixerSliders[0].value = sliderValue;
            mixerSliders[0].value = 3;
            //audioMixer.SetFloat("MasterVolume", audioValue);
            audioMixer.SetFloat("Master", -10);

            // ----- Declarar volume Musica ----- \\
            //CheckPrefs("MusicaVolume", out audioValue, out sliderValue);
            //mixerSliders[1].value = sliderValue;
            mixerSliders[1].value = 3;
            //audioMixer.SetFloat("MusicaVolume", audioValue);
            audioMixer.SetFloat("Musica", -10);

            // ----- Declarar volume Efeito ----- \\
            //CheckPrefs("EfeitosVolume", out audioValue, out sliderValue);
            //mixerSliders[2].value = sliderValue;
            mixerSliders[2].value = 3;
            //audioMixer.SetFloat("EfeitosVolume", audioValue);
            audioMixer.SetFloat("Efeitos", -10);
            startPlay = false;
            //this.gameObject.SetActive(false);
        }
        
    }

    private void CheckPrefs(string _name, out int audioValue, out int sliderValue)
    {
        sliderValue = 0;
        Debug.Log(_name + " - " + PlayerPrefs.GetInt(_name));
        if (PlayerPrefs.HasKey(_name))
        {
            audioValue = PlayerPrefs.GetInt(_name);
            switch (audioValue)
            {
                case -88:
                    sliderValue = 0;
                    break;

                case -40:
                    audioValue = 1;
                    break;

                case -20:
                    audioValue = 2;
                    break;

                case -10:
                    audioValue = 3;
                    break;

                case 5:
                    audioValue = 4;
                    break;
            }
        }
        else
        {
            audioValue = -20;
            sliderValue = 2;
        }
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
        //PlayerPrefs.SetInt("MasterVolume", value);
        //PlayerPrefs.Save();

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
        //PlayerPrefs.SetInt("MusicaVolume", value);
        //PlayerPrefs.Save();
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
        //PlayerPrefs.SetInt("EfeitosVolume", value);
        //PlayerPrefs.Save();
    }

}
