using UnityEngine;
using System.Collections.Generic;

public class CameraVisionObstacle : MonoBehaviour
{
    [Header("Réglages")]
    public Transform player;          // Glisse Keisha ici
    public LayerMask wallLayer;      // Sélectionne le Layer de tes murs
    public float transparentAlpha = 0.3f; // Niveau de transparence (0 = invisible)

    private List<Renderer> currentlyTransparent = new List<Renderer>();
    private Dictionary<Renderer, Color> originalColors = new Dictionary<Renderer, Color>();

    void Update()
    {
        if (player == null) return;

        // On calcule la direction entre la caméra et Keisha
        Vector3 direction = player.position - transform.position;
        float distance = Vector3.Distance(transform.position, player.position);

        // On lance un rayon (Raycast) pour voir ce qui bloque
        RaycastHit[] hits = Physics.RaycastAll(transform.position, direction, distance, wallLayer);

        // On crée une liste des renderers touchés ce coup-ci
        List<Renderer> hitsThisFrame = new List<Renderer>();

        foreach (var hit in hits)
        {
            Renderer rend = hit.collider.GetComponent<Renderer>();
            if (rend != null)
            {
                hitsThisFrame.Add(rend);

                // Si on ne l'avait pas déjà rendu transparent
                if (!currentlyTransparent.Contains(rend))
                {
                    MakeTransparent(rend);
                    currentlyTransparent.Add(rend);
                }
            }
        }

        // On remet en opaque ceux qui ne sont plus dans le chemin
        for (int i = currentlyTransparent.Count - 1; i >= 0; i--)
        {
            Renderer rend = currentlyTransparent[i];
            if (!hitsThisFrame.Contains(rend))
            {
                ResetOpacity(rend);
                currentlyTransparent.RemoveAt(i);
            }
        }
    }

    void MakeTransparent(Renderer rend)
    {
        if (!originalColors.ContainsKey(rend))
            originalColors.Add(rend, rend.material.color);

        Color c = rend.material.color;
        c.a = transparentAlpha;
        rend.material.color = c;
    }

    void ResetOpacity(Renderer rend)
    {
        if (originalColors.ContainsKey(rend))
        {
            rend.material.color = originalColors[rend];
        }
    }
}