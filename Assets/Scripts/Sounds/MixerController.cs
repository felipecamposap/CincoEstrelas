using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MixerController : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    

    public void MasterVolume(Slider _slider)
    {
        audioMixer.SetFloat("Master", _slider.value);
        PlayerPrefs.SetFloat("MasterVolume", _slider.value);

    }

    public void MusicaVolume(Slider _slider)
    {
        audioMixer.SetFloat("Musica", _slider.value);
        PlayerPrefs.SetFloat("MusicaVolume", _slider.value);
    }

    public void EfeitoVolume(Slider _slider)
    {
        audioMixer.SetFloat("Efeito", _slider.value);
        PlayerPrefs.SetFloat("EfeitoVolume", _slider.value);
    }


}
