using UnityEngine;
using UnityEngine.UI;

public class NextSoundPlayer : MonoBehaviour
{
    [Header("Liste des sons à jouer")]
    public AudioClip[] sounds;

    [Header("AudioSource qui jouera les sons")]
    public AudioSource audioSource;

    private int currentIndex = 0;

    public void PlayNextSound()
    {
        if (sounds.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Aucun son ou AudioSource manquant !");
            return;
        }

        // Assigne le clip du son actuel
        audioSource.clip = sounds[currentIndex];
        audioSource.Play(); // joue depuis le début

        // Passe au son suivant pour le prochain clic
        currentIndex++;
        if (currentIndex >= sounds.Length)
        {
            currentIndex = 0;
        }
    }

    public void PlayPreviousSound()
    {
        if (sounds.Length == 0 || audioSource == null)
        {
            Debug.LogWarning("Aucun son ou AudioSource manquant !");
            return;
        }

        // Décrémente l'index pour revenir au son précédent
        currentIndex--;
        if (currentIndex < 0)
        {
            currentIndex = sounds.Length - 1; // boucle vers la fin
        }

        // Assigne le clip et joue depuis le début
        audioSource.clip = sounds[currentIndex];
        audioSource.Play();
    }
}
