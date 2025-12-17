using UnityEngine;

public class FloatingPickup : MonoBehaviour
{
    [Header("Float")]
    public float floatAmplitude = 0.15f;   // hauteur du flottement
    public float floatSpeed = 2f;          // vitesse du flottement

    [Header("Rotate")]
    public Vector3 rotationSpeed = new Vector3(0f, 90f, 0f); // degrés/sec

    [Header("Options")]
    public bool useWorldSpace = true;      // si l'objet bouge, laisse true

    private Vector3 startPos;
    private float randomOffset;

    void Start()
    {
        startPos = transform.position;
        randomOffset = Random.Range(0f, 10f); // pour que tous ne flottent pas synchro
    }

    void Update()
    {
        // rotation
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

        // flottement
        float yOffset = Mathf.Sin((Time.time + randomOffset) * floatSpeed) * floatAmplitude;

        if (useWorldSpace)
        {
            transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
        }
        else
        {
            transform.localPosition = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
        }
    }

    // Si tu déplaces l'objet en runtime (spawn), tu peux appeler ça
    public void ResetStartPosition()
    {
        startPos = transform.position;
    }
}
