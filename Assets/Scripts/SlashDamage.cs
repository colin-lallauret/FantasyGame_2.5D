using UnityEngine;

public class SlashDamage : MonoBehaviour
{
    public GameObject hitVFX;
    public AudioClip hitSound;
    public float vfxDuration = 2f;

    private void OnTriggerEnter(Collider other)
    {
        // On vérifie si l'objet touché est un Ennemi, un Fantôme ou le Boss
        if (other.CompareTag("Enemy") || other.CompareTag("Ghost") || other.CompareTag("Boss"))
        {
            // Calculer la direction du recul (du joueur vers l'objet touché)
            Vector3 knockbackDir = (other.transform.position - transform.position).normalized;
            knockbackDir.y = 0.2f; // Petit saut vers le haut pour l'effet visuel

            // --- Effets visuels et sonores ---
            if (hitVFX != null)
            {
                GameObject vfx = Instantiate(hitVFX, other.transform.position, Quaternion.identity);
                Destroy(vfx, vfxDuration);
            }

            if (hitSound != null)
                AudioSource.PlayClipAtPoint(hitSound, other.transform.position);

            // --- Gestion des dégâts par type ---
            
            // 1. Cas de l'ennemi normal (3 PV)
            if (other.CompareTag("Enemy"))
            {
                EnemyHealth enHealth = other.GetComponent<EnemyHealth>();
                if (enHealth != null)
                {
                    enHealth.TakeDamage(knockbackDir);
                }
            }
            // 2. Cas du Boss (5 PV)
            else if (other.CompareTag("Boss"))
            {
                BossHealth bHealth = other.GetComponent<BossHealth>();
                if (bHealth != null)
                {
                    bHealth.TakeDamage(knockbackDir);
                }
            }
            // 3. Cas du Fantôme (mort instantanée)
            else if (other.CompareTag("Ghost"))
            {
                Destroy(other.gameObject);
            }

            // Effet de tremblement de caméra
            if (CinemachineShake.instance != null)
                CinemachineShake.instance.TriggerShake(1f, 2f, 0.1f);
        }
    }
}