using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public PlayerInventory inventory;   
    public GameObject slashPrefab;      
    public Transform slashPoint;        
    public float slashDuration = 1f;    

    [Header("Cooldown")]
    public float cooldownTime = 1.0f;   
    private float cooldownTimer = 0f;

    [Header("Sound")]
    public AudioSource audioSource; 
    public AudioClip slashSound;

    void Update()
    {
        if (cooldownTimer > 0f)
            cooldownTimer -= Time.deltaTime;

        // ✅ Clic gauche (0) et vérification de l'épée
        if (inventory != null && inventory.HasSword &&
            Input.GetMouseButtonDown(0) && cooldownTimer <= 0f)
        {
            PlaySlash();
            cooldownTimer = cooldownTime;
        }
    }

    void PlaySlash()
    {
        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        GameObject slash = Instantiate(slashPrefab, slashPoint.position, slashPoint.rotation);
        Destroy(slash, slashDuration);
    }
}