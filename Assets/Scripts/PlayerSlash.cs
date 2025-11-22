using UnityEngine;

public class PlayerSlash : MonoBehaviour
{
    public PlayerInventory inventory;   // ton script d’inventaire
    public GameObject slashPrefab;      // prefab de slash VFX
    public Transform slashPoint;        // point devant ton player
    public float slashDuration = 1f;    // durée avant destruction

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

        if (inventory != null && inventory.HasSword &&
            Input.GetKeyDown(KeyCode.Space) && cooldownTimer <= 0f)
        {
            PlaySlash();
            cooldownTimer = cooldownTime;
        }
    }

    void PlaySlash()
    {
        // 🔊 Son de slash
        if (audioSource != null && slashSound != null)
        {
            audioSource.PlayOneShot(slashSound);
        }

        // 💥 VFX slash
        GameObject slash = Instantiate(slashPrefab, slashPoint.position, slashPoint.rotation);
        Destroy(slash, slashDuration);
    }
}
