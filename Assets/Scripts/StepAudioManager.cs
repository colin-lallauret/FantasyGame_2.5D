using UnityEngine;

public class StepAudioManager : MonoBehaviour
{
    public static StepAudioManager instance;

    [Header("Audio Source")]
    public AudioSource audioSource;

    [Header("Step Voice Clips (size = 6)")]
    public AudioClip[] stepClips = new AudioClip[6];

    [Header("Voice Volume")]
    [Range(0f, 1f)]
    public float voiceVolume = 1f; // 👈 jauge volume voix

    private bool[] stepPlayed = new bool[6];

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        // ✅ Étape 1 auto au lancement
        PlayStep(1);
    }

    public void PlayStep(int stepNumber)
    {
        int index = stepNumber - 1;

        if (index < 0 || index >= stepClips.Length)
        {
            Debug.LogWarning("StepAudioManager : step invalide = " + stepNumber);
            return;
        }

        if (stepClips[index] == null)
        {
            Debug.LogWarning("StepAudioManager : aucun clip assigné pour step " + stepNumber);
            return;
        }

        // 🔊 Joue la voix avec volume réglable
        audioSource.PlayOneShot(stepClips[index], voiceVolume);

        Debug.Log($"🔊 Step {stepNumber} joué (Volume = {voiceVolume})");
    }
}
