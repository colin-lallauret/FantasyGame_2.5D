using UnityEngine;

public class BossAttack : MonoBehaviour
{
    [Header("Settings")]
    public int damageAmount = 1; // Tu peux régler le nombre de dégâts ici

    private void OnCollisionEnter(Collision collision)
    {
        // On vérifie si l'objet touché est le Joueur
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            
            if (playerHealth != null)
            {
                // ✅ On ajoute l'argument (1) demandé par PlayerHealth
                playerHealth.TakeDamage(damageAmount); 
            }
        }
    }
}