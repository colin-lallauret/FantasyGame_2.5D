using UnityEngine;
using System.Collections.Generic;

public class ObstacleTransparency : MonoBehaviour
{
    [Header("Cibles")]
    public Transform player;          
    public LayerMask wallLayer;       

    [Header("Réglages")]
    public float transparentAlpha = 0.3f; 
    public float fadeSpeed = 5f;          // La vitesse de la transition
    public string targetTag = "Wall";     

    // On utilise un dictionnaire pour garder une trace de l'alpha cible de chaque arbre
    private Dictionary<SpriteRenderer, float> targetAlphas = new Dictionary<SpriteRenderer, float>();
    private List<SpriteRenderer> activeTrees = new List<SpriteRenderer>();

    void Update()
    {
        if (player == null) return;

        // Ta logique de Raycast d'origine
        Vector3 direction = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);

        // On détecte les obstacles
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, direction, distance, wallLayer);

        // On crée une liste temporaire pour savoir qui est touché à cette frame
        HashSet<SpriteRenderer> currentlyHit = new HashSet<SpriteRenderer>();

        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag(targetTag))
            {
                SpriteRenderer sr = hit.collider.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    currentlyHit.Add(sr);
                    if (!activeTrees.Contains(sr)) activeTrees.Add(sr);
                    targetAlphas[sr] = transparentAlpha; // On veut qu'il soit transparent
                }
            }
        }

        // On boucle sur tous les arbres qu'on a un jour touchés
        for (int i = activeTrees.Count - 1; i >= 0; i--)
        {
            SpriteRenderer sr = activeTrees[i];

            if (sr == null) {
                activeTrees.RemoveAt(i);
                continue;
            }

            // Si l'arbre n'est plus dans "currentlyHit", sa cible redevient 1 (opaque)
            float target = currentlyHit.Contains(sr) ? transparentAlpha : 1f;
            
            // TRANSITION DOUCE (L'amélioration principale)
            Color c = sr.color;
            c.a = Mathf.MoveTowards(c.a, target, Time.deltaTime * fadeSpeed);
            sr.color = c;

            // Si l'arbre est revenu à 100% d'opacité, on arrête de calculer pour lui
            if (!currentlyHit.Contains(sr) && c.a >= 1f)
            {
                activeTrees.RemoveAt(i);
            }
        }
    }
}