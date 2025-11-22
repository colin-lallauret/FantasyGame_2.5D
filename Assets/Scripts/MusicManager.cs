using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Musics")]
    public AudioClip musicStart;
    public AudioClip musicDoorOpen;

    [Header("Reward Sound")]
    public AudioClip rewardSound;   // 👈 nouveau son

    [Header("Volume (0 à 1)")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        ApplyVolume();
        PlayStartMusic();
    }

    void Update()
    {
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        if (audioSource != null)
            audioSource.volume = musicVolume;
    }

    public void PlayStartMusic()
    {
        audioSource.clip = musicStart;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDoorOpenMusic()
    {
        audioSource.clip = musicDoorOpen;
        audioSource.loop = true;
        audioSource.Play();
    }

    // 👇 POUR JOUER LE SON DE REWARD
    public void PlayRewardSound()
    {
        if (audioSource != null && rewardSound != null)
            audioSource.PlayOneShot(rewardSound);
    }

    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }
}
