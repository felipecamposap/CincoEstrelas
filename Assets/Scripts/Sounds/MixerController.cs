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
            // ----- Declarar volume Geral ----- \\
            CheckPrefs("Master", out var audioValue, out var sliderValue);
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
        sliderValue = audioValue switch
        {
            -88 => 0,
            -40 => 1,
            -20 => 2,
            -10 => 3,
            5 => 4,
            _ => sliderValue
        };
        //}
        //else
        //{
        //    audioValue = -20;
        //    sliderValue = 2;
        //}
    }

    public void MasterVolume()
    {
        var value = mixerSliders[0].value switch
        {
            0 => -88,
            1 => -40,
            2 => -20,
            3 => -10,
            4 => 5,
            _ => 0
        };

        audioMixer.SetFloat("Master", value);
    }

    public void MusicaVolume()
    {
        var value = mixerSliders[1].value switch
        {
            0 => -88,
            1 => -40,
            2 => -20,
            3 => -10,
            4 => 5,
            _ => 0
        };

        audioMixer.SetFloat("Musica", value);
    }

    public void EfeitoVolume()
    {
        var value = mixerSliders[2].value switch
        {
            0 => -88,
            1 => -40,
            2 => -20,
            3 => -10,
            4 => 5,
            _ => 0
        };

        audioMixer.SetFloat("Efeitos", value);
    }

}
