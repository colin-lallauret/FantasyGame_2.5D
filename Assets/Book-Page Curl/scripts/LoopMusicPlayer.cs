using UnityEngine;

public class LoopMusicPlayer : MonoBehaviour
{
    [Header("Liste des sons à jouer (Next/Previous)")]
    public AudioClip[] sounds;

    [Header("AudioSource qui joue les sons contrôlés")]
    public AudioSource audioSource;

    [Header("Musique de fond")]
    public AudioSource backgroundSource;
    public AudioClip backgroundMusic;

    private int currentIndex = 0;

    private void Start()
    {
        currentIndex = 0;

        // Lance la musique de fond si disponible
        if (backgroundSource != null && backgroundMusic != null)
        {
            backgroundSource.clip = backgroundMusic;
            backgroundSource.loop = true;
            backgroundSource.Play();
        }
    }

    public void PlayNextSound()
    {
        if (sounds.Length == 0 || audioSource == null) return;

        audioSource.clip = sounds[currentIndex];
        audioSource.time = 0f;
        audioSource.Play();

        currentIndex++;
        if (currentIndex >= sounds.Length)
            currentIndex = 0;
    }

    public void PlayPreviousSound()
    {
        if (sounds.Length == 0 || audioSource == null) return;

        currentIndex--;
        if (currentIndex < 0)
            currentIndex = sounds.Length - 1;

        audioSource.clip = sounds[currentIndex];
        audioSource.time = 0f;
        audioSource.Play();
    }
}
