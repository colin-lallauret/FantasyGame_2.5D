using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    [Header("Santé & Combat")]
    public int health = 5;
    public float knockbackForce = 3f;
    private Rigidbody rb;
    private bool isKnockedBack = false;

    [Header("Animation Procédurale")]
    public SpriteRenderer sr;
    public float flipInterval = 0.5f; // Temps entre chaque flip
    private float flipTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        
        // Empêche le boss de tomber sur le côté
        if (rb != null) rb.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    void Update()
    {
        // Animation de Flip automatique pour donner de la vie
        flipTimer += Time.deltaTime;
        if (flipTimer >= flipInterval)
        {
            sr.flipX = !sr.flipX; // Inverse l'état actuel
            flipTimer = 0;
        }
    }

    public void TakeDamage(Vector3 direction)
    {
        if (isKnockedBack) return;
        health--;

        if (health <= 0)
        {
            Die();
        }
        else
        {
            StartCoroutine(Knockback(direction));
        }
    }

    IEnumerator Knockback(Vector3 dir)
    {
        isKnockedBack = true;
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(dir * knockbackForce, ForceMode.Impulse);
        }
        yield return new WaitForSeconds(0.2f);
        isKnockedBack = false;
    }

    void Die()
    {
        if (EnemyManager.instance != null)
        {
            EnemyManager.instance.BossDied();
        }
        Destroy(gameObject);
    }
}