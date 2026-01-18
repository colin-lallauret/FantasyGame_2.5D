using UnityEngine;
using System.Collections;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Musics")]
    public AudioClip musicStart;
    public AudioClip musicDoorOpen;

    [Header("Reward & Victory Sounds")]
    public AudioClip rewardSound;   // Son du portail
    public AudioClip victorySound;  // Son de victoire (fanfare)

    [Header("Volume (0 à 1)")]
    [Range(0f, 1f)]
    public float musicVolume = 1f;

    private bool isMusicStopped = false;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        isMusicStopped = false;
        ApplyVolume();
        PlayStartMusic();
    }

    void Update()
    {
        if (isMusicStopped) return;
        ApplyVolume();
    }

    public void ApplyVolume()
    {
        if (audioSource != null)
            audioSource.volume = musicVolume;
    }

    public void PlayStartMusic()
    {
        if (isMusicStopped || audioSource == null) return;
        audioSource.clip = musicStart;
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PlayDoorOpenMusic()
    {
        if (isMusicStopped || audioSource == null) return;
        audioSource.clip = musicDoorOpen;
        audioSource.loop = true;
        audioSource.Play();
    }

    // --- Séquence de Victoire ---

    public void StartVictorySequence()
    {
        StartCoroutine(VictoryRoutine());
    }

    private IEnumerator VictoryRoutine()
    {
        // 1. On coupe la musique du combat
        StopMusic();

        // ✅ 2. On force le volume au maximum pour la fanfare
        musicVolume = 1f;
        ApplyVolume();
        
        // 3. Jouer le son de victoire
        if (victorySound != null)
        {
            audioSource.PlayOneShot(victorySound);
            yield return new WaitForSeconds(victorySound.length);
        }

        // 4. Jouer le son du portail
        if (rewardSound != null)
        {
            audioSource.PlayOneShot(rewardSound);
        }

        // 5. On active le portail dans l'EnemyManager
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.ActivatePortal();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null)
            audioSource.Stop();
    }

    public void StopAllMusic()
    {
        isMusicStopped = true;

        // ✅ On force le volume au maximum avant de couper la musique de fond
        musicVolume = 0.5f;
        ApplyVolume();

        if (audioSource != null)
        {
            audioSource.Stop();
        }
        
        this.enabled = false; 
        Debug.Log("MusicManager : Volume forcé à 1 et musique stoppée pour le GameOver.");
    }
}