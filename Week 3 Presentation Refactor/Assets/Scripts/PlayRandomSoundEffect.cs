using UnityEngine;
using System.Collections;

public class PlayRandomSoundEffect : MonoBehaviour
{
    private AudioSource audioSource;
    public AudioClip[] noises; // Noises clips for randomly playing
    public AudioClip[] deathNoises; // For random selection on death
    private AudioClip noise;

    private float timeElapsed;
    private float randomTimeForSound;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        randomTimeForSound = Random.Range(1, 3);
    }
    void Update()
    {
        if (timeElapsed >= randomTimeForSound)
        {
            int index = Random.Range(0, noises.Length);
            audioSource.clip = noises[index];
            audioSource.Play();
            timeElapsed = 0;
            randomTimeForSound = Random.Range(10, 20);
        }
        else {
            timeElapsed += Time.deltaTime;
        }
    }

    // Callback for death sound
    public void PlayDeathSound() {
        audioSource.Stop(); // In case it was already making a sound

        // Death sound
        int index = Random.Range(0, deathNoises.Length);
        audioSource.clip = deathNoises[index];
        audioSource.Play();
    }
}
