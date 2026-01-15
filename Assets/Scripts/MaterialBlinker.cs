using UnityEngine;

public class MaterialBlinker : MonoBehaviour
{
    [Header("Références")]
    [SerializeField] private Renderer objectRenderer;

    [Header("Paramètres de clignotement")]
    [SerializeField] private float blinkInterval = 0.5f;

    private Material materialInstance;
    private Color baseColor;
    private bool isVisible = true;

    private void Awake()
    {
        if (objectRenderer == null)
        {
            objectRenderer = GetComponent<Renderer>();
        }

        // IMPORTANT : instancier le material pour éviter de modifier l’original
        materialInstance = objectRenderer.material;
        baseColor = materialInstance.color;
    }

    private void Start()
    {
        InvokeRepeating(nameof(ToggleBlink), blinkInterval, blinkInterval);
    }

    private void ToggleBlink()
    {
        isVisible = !isVisible;

        Color newColor = baseColor;
        newColor.a = isVisible ? 1f : 0f;

        materialInstance.color = newColor;
    }

    private void OnDestroy()
    {
        CancelInvoke();
    }
}
