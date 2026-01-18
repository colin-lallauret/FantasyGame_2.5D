using UnityEngine;

public class ValidationTrigger : MonoBehaviour
{
    public int stepNumber; // Mets 1, 2 ou 3 dans l'inspecteur selon la zone
    private PathManager pathManager;

    void Start()
    {
        pathManager = Object.FindAnyObjectByType<PathManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Vérifie si c'est Keisha (le Player) qui touche la zone
        if (other.CompareTag("Player"))
        {
            pathManager.ReachValidation(stepNumber);
            // On détruit la zone pour ne pas qu'elle se réactive
            Destroy(gameObject); 
        }
    }
    
    // Si tu es en 3D, utilise OnTriggerEnter à la place
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pathManager.ReachValidation(stepNumber);
            Destroy(gameObject);
        }
    }
}