using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip[] clips;
    [SerializeField] private AudioSource audioSource;
    private bool canChangeMusic = true;
    private byte clipIndex;
    private float timeMusic;


    // Start is called before the first frame update
    private void Start()
    {
        GameController.controller.audioSource = audioSource;
        audioSource = GetComponent<AudioSource>();
        clipIndex = (byte)Random.Range(0, clips.Length);
        audioSource.clip = clips[clipIndex];
        timeMusic = clips[clipIndex].length;
        audioSource.Play();
    }

    // Update is called once per frame
    private void Update()
    {
        if (audioSource.time >= audioSource.clip.length)
        {
            clipIndex++;
            if (clipIndex == clips.Length)
                clipIndex = 0;
            //GameController.controller.uiController.StartClipAnimation(audioSource.clip.ToString());
            audioSource.clip = clips[clipIndex];
            audioSource.Play();

        }
        //Debug.Log(audioSource.time >= audioSource.clip.length);
        //if (!audioSource.isPlaying)
        //{
        //    if (canChangeMusic)
        //    {
        //        canChangeMusic = false;

        //        audioSource.clip = clips[clipIndex];
        //        audioSource.Play();
        //        clipIndex++;
        //        if(clipIndex == clips.Length)
        //            clipIndex = 0;
        //        GameController.controller.uiController.StartClipAnimation(audioSource.clip.ToString());
        //    }
        //}else{
        //    canChangeMusic = true;

        //}
    }

}
