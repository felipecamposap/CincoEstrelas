using UnityEngine;
using System;
using System.Threading;

public class AudioPlayer : MonoBehaviour
{
    public Sound[] musicSounds, sfxSounds;
    public AudioClip[] footstepSounds;
    public AudioSource musicSource, footstepSource;
    public static AudioPlayer instance;

    private float fadeDuration;
    private float startVolume;
    private float targetVolume;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        startVolume = musicSource.volume;
        musicSource.volume = startVolume;
    }

    private void Start()
    {
        PlayMusic("MainTheme", true, false);
        PlaySFX("AmbientSound");
    }

    public void PlayMusic(string name, bool loop = true, bool crossfade = true)
    {
        musicSource.loop = loop;

        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Music Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name, AudioSource source = null)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("SFX Not Found");
        }
        else if (!s.source)
        {
            if (musicSource) musicSource.PlayOneShot(s.clip);
        }
        else
        {
            s.source.PlayOneShot(s.clip);
        }
    }

    public void PlayFootstepSound()
    {
        footstepSource.PlayOneShot(footstepSounds[UnityEngine.Random.Range(0, footstepSounds.Length)]);
    }
}