using UnityEngine;
using UnityEngine.UI;

public class PlayerInventory : MonoBehaviour
{
    [Header("Slot 1 : Sword Settings")]
    public Image swordIconImage; // Glisse ici l'UI de la CASE 1
    public Sprite swordIcon;
    private bool hasSword = false;

    [Header("Slot 2 : Key Settings")]
    public Image keyIconImage;   // Glisse ici l'UI de la CASE 2
    public Sprite keyIcon;
    private bool hasKey = false;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip pickupSound;

    private void OnTriggerEnter(Collider other)
    {
        // --- SWORD (Case 1) ---
        if (other.CompareTag("Sword") && !hasSword)
        {
            PickupSword(other.gameObject);
        }

        // --- KEY (Case 2) ---
        if (other.CompareTag("Key") && !hasKey)
        {
            PickupKey(other.gameObject);
        }
    }

    void PickupSword(GameObject swordObject)
    {
        // On active l'image de la Case 1 avec le sprite de l'épée
        swordIconImage.sprite = swordIcon;
        swordIconImage.enabled = true;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        Destroy(swordObject);
        hasSword = true;
    }

    void PickupKey(GameObject keyObject)
    {
        // On active l'image de la Case 2 avec le sprite de la clé
        keyIconImage.sprite = keyIcon;
        keyIconImage.enabled = true;

        if (audioSource != null && pickupSound != null)
            audioSource.PlayOneShot(pickupSound);

        Destroy(keyObject);
        hasKey = true;
    }

    public bool HasKey => hasKey;
    public bool HasSword => hasSword;
}