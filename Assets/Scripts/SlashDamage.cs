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
        if (other.CompareTag("Enemy"))
        {
            // Spawn VFX de mort
            if (hitVFX != null)
            {
                GameObject vfx = Instantiate(hitVFX, other.transform.position, Quaternion.identity);
                Destroy(vfx, vfxDuration);
            }

            // Joue le son
            if (hitSound != null)
            {
                AudioSource.PlayClipAtPoint(hitSound, other.transform.position);
            }

            // Camera shake via CinemachineShake
            if (CinemachineShake.instance != null)
            {
                CinemachineShake.instance.TriggerShake(
                    shakeAmplitude,
                    shakeFrequency,
                    shakeDuration
                );
            }

            // ✅ prévenir le manager
            if (EnemyManager.instance != null)
                EnemyManager.instance.EnemyDied();

            // Détruit l'ennemi
            Destroy(other.gameObject);
        }
    }
}
