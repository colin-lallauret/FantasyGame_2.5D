using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    public GameObject hitVFX;
    public AudioClip hitSound;
    public float vfxDuration = 2f;

    [Header("Camera Shake (Cinemachine 3)")]
    public float shakeAmplitude = 1f;
    public float shakeFrequency = 2f;
    public float shakeDuration = 0.12f;

    private void OnTriggerEnter(Collider other)
    {
        bool isEnemy = other.CompareTag("Enemy");
        bool isGhost = other.CompareTag("Ghost");

        if (!isEnemy && !isGhost)
            return;

        // VFX
        if (hitVFX != null)
        {
            GameObject vfx = Instantiate(hitVFX, other.transform.position, Quaternion.identity);
            Destroy(vfx, vfxDuration);
        }

        // Son
        if (hitSound != null)
        {
            AudioSource.PlayClipAtPoint(hitSound, other.transform.position);
        }

        // Camera shake
        if (CinemachineShake.instance != null)
        {
            CinemachineShake.instance.TriggerShake(
                shakeAmplitude,
                shakeFrequency,
                shakeDuration
            );
        }

        // 🔥 Compte uniquement les ENEMY (pas les Ghost)
        if (isEnemy && EnemyManager.instance != null)
        {
            EnemyManager.instance.EnemyDied();
        }

        // Détruit l'objet touché (Enemy ou Ghost)
        Destroy(other.gameObject);
    }
}
