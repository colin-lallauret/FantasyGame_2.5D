using UnityEngine;

public class DoorInteraction : MonoBehaviour
{
    public GameObject doorClose;
    public GameObject doorOpen;

    public AudioSource audioSource;
    public AudioClip wrongSound;
    public AudioClip successSound;

    private bool isOpened = false;

    private void Start()
    {
        doorOpen.SetActive(false);
        doorClose.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOpened) return;

        if (other.CompareTag("Player"))
        {
            PlayerInventory inv = other.GetComponent<PlayerInventory>();
            if (inv == null)
            {
                Debug.LogError("Le Player n’a pas PlayerInventory !");
                return;
            }

            if (inv.HasKey)
            {
                OpenDoor();
            }
            else
            {
                PlayWrong();
            }
        }
    }

    void PlayWrong()
    {
        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);
    }

    void OpenDoor()
    {
        isOpened = true;

        // Son d'ouverture
        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);

        // 🔥 Changer la musique via MusicManager
        if (MusicManager.instance != null)
        {
            MusicManager.instance.PlayDoorOpenMusic();
        }

        // Activer / désactiver les portes
        doorClose.SetActive(false);
        doorOpen.SetActive(true);
    }
}
