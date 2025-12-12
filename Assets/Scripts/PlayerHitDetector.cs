using UnityEngine;

public class PlayerHitDetector : MonoBehaviour
{
    private PlayerHealth playerHealth;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (playerHealth == null) return;

        if (other.CompareTag("Enemy") || other.CompareTag("Ghost"))
        {
            Debug.Log("Keisha touchée par : " + other.name);
            playerHealth.TakeDamage(1);
        }
    }
}
