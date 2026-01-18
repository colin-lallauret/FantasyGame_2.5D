using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int health = 3;
    public float knockbackForce = 5f;
    public float knockbackDuration = 0.2f;

    private Rigidbody rb;
    private bool isKnockedBack = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // On s'assure que le Rigidbody peut bouger pour le knockback
        if (rb != null)
        {
            rb.isKinematic = false;
            rb.constraints = RigidbodyConstraints.FreezeRotation;
        }
    }

    public void TakeDamage(Vector3 damageDirection)
    {
        if (isKnockedBack) return;

        health--;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            // Appliquer le Knockback
            StartCoroutine(KnockbackRoutine(damageDirection));
        }
    }

    private IEnumerator KnockbackRoutine(Vector3 direction)
    {
        isKnockedBack = true;
        
        // On désactive le script de mouvement pour ne pas contrer le recul
        var moveScript = GetComponent<EnemyFollow>();
        if (moveScript != null) moveScript.enabled = false;

        // Force de recul
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero; // Stop la vitesse actuelle
            rb.AddForce(direction.normalized * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(knockbackDuration);

        if (moveScript != null) moveScript.enabled = true;
        isKnockedBack = false;
    }

    void Die()
    {
        if (EnemyManager.instance != null)
            EnemyManager.instance.EnemyDied();

        Destroy(gameObject);
    }
}