using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Slider = UnityEngine.UI.Slider;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider[] mixerSliders;
    [SerializeField] private bool startPlay;


    public void Start()
    {

        if (GameController.controller.startSound)
        {
            // ----- Declarar volume Geral ----- \\
            mixerSliders[0].value = 3;
            audioMixer.SetFloat("Master", -10);
            GameController.controller.soundValues[0] = 3;

            // ----- Declarar volume Musica ----- \\
            mixerSliders[1].value = 3;
            audioMixer.SetFloat("Musica", -10);
            GameController.controller.soundValues[1] = 3;

            // ----- Declarar volume Efeito ----- \\
            mixerSliders[2].value = 3;
            audioMixer.SetFloat("Efeitos", -10);
            GameController.controller.soundValues[2] = 3;

            GameController.controller.startSound = false;
            
        } 
        else
        {
            // ----- Declarar volume Geral ----- \\
            mixerSliders[0].value = GameController.controller.soundValues[0];
            MasterVolume();

            // ----- Declarar volume Musica ----- \\
            mixerSliders[1].value = GameController.controller.soundValues[1];
            MusicaVolume();

            // ----- Declarar volume Efeito ----- \\
            mixerSliders[2].value = GameController.controller.soundValues[2];
            EfeitoVolume();
        }
        if(startPlay)
            this.gameObject.SetActive(false);
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
        GameController.controller.soundValues[0] = (int)mixerSliders[0].value;
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
        GameController.controller.soundValues[1] = (int)mixerSliders[1].value;
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
        GameController.controller.soundValues[2] = (int)mixerSliders[2].value;
    }

}
