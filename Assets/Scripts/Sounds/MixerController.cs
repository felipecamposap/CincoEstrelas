using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] Slider[] mixerSliders;


    public void Start()
    {
        int audioValue;
        int sliderValue;

        // ----- Declarar volume Geral ----- \\
        CheckPrefs("MasterVolume", out audioValue, out sliderValue);
        mixerSliders[0].value = sliderValue;
        audioMixer.SetFloat("MasterVolume", audioValue);

        // ----- Declarar volume Musica ----- \\
        CheckPrefs("MusicaVolume", out audioValue, out sliderValue);
        mixerSliders[1].value = sliderValue;
        audioMixer.SetFloat("MusicaVolume", audioValue);

        // ----- Declarar volume Efeito ----- \\
        CheckPrefs("EfeitosVolume", out audioValue, out sliderValue);
        mixerSliders[2].value = sliderValue;
        audioMixer.SetFloat("EfeitosVolume", audioValue);

    }

    private void CheckPrefs(string _name, out int audioValue, out int sliderValue)
    {
        sliderValue = 0;
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
        float value = 0;
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
        PlayerPrefs.SetFloat("MasterVolume", value);

    }

    public void MusicaVolume()
    {
        float value = 0;
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
        PlayerPrefs.SetFloat("MusicaVolume", value);
    }

    public void EfeitoVolume()
    {
        float value = 0;
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
        PlayerPrefs.SetFloat("EfeitosVolume", value);
    }


}
