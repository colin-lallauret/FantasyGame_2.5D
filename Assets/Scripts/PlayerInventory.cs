using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Key Settings")]
    public Image keyIconImage;
    public Sprite keyIcon;
    private bool hasKey = false;

    [Header("Sword Settings")]
    public Image swordIconImage;
    public Sprite swordIcon;
    private bool hasSword = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // --- KEY ---
        if (other.CompareTag("Key") && !hasKey)
        {
            PickupKey(other.gameObject);
        }

        // --- SWORD ---
        if (other.CompareTag("Sword") && !hasSword)
        {
            PickupSword(other.gameObject);
        }
    }

    void PickupKey(GameObject keyObject)
    {
        keyIconImage.sprite = keyIcon;
        keyIconImage.enabled = true;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        Destroy(keyObject);
        hasKey = true;
    }

    void PickupSword(GameObject swordObject)
    {
        swordIconImage.sprite = swordIcon;
        swordIconImage.enabled = true;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        Destroy(swordObject);
        hasSword = true;
    }

    public bool HasKey => hasKey;
    public bool HasSword => hasSword;
}
