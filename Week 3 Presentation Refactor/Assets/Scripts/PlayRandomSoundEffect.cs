using UnityEngine;
using System.Collections;

public class PlayRandomSoundEffect : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] noises;
    private AudioClip noise;

    private float timeElapsed;
    private float randomTimeForSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        randomTimeForSound = Random.Range(5, 15);
    }
    void Update()
    {
        if (timeElapsed >= randomTimeForSound)
        {
            int index = Random.Range(0, noises.Length);
            audioSource.clip = noises[index];
            audioSource.Play();
            timeElapsed = 0;
            randomTimeForSound = Random.Range(10, 30);
        }
        else {
            timeElapsed += Time.deltaTime;
            print(timeElapsed);
        }
    }
}
