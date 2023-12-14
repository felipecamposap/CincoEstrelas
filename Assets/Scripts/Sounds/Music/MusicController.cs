using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    private bool canChangeMusic = true;
    private byte clipIndex;


    // Start is called before the first frame update
    void Start()
    {
        clipIndex = (byte)Random.Range(0, clips.Length);
        audioSource = GetComponent<AudioSource>();
        //try { GameController.controller.audioSource = audioSource; }
        //catch { Debug.Log("AudioSource GameController não encontrado!"); }
    }

    // Update is called once per frame
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            if (canChangeMusic)
            {
                canChangeMusic = false;
                audioSource.clip = clips[clipIndex];
                audioSource.Play();
                clipIndex++;
                if(clipIndex == clips.Length)
                    clipIndex = 0;
                GameController.controller.uiController.StartClipAnimation(audioSource.clip.ToString());
            }
        }else{
            canChangeMusic = true;

        }

        if(Input.GetButtonDown("MudarMusica"))
            audioSource.Stop();

    }

}
