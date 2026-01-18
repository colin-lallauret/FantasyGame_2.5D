using UnityEngine;

public class IdleBob : MonoBehaviour
{
    [Header("Bobbing")]
    [SerializeField] float amplitude = 1f;     // en pixels
    [SerializeField] float frequency = 1.5f;

    [Header("Pixel Settings")]
    [SerializeField] int pixelsPerUnit = 16;

    [Header("Idle Detection")]
    [SerializeField] float idleThreshold = 0.01f;

    Transform playerRoot;
    Vector3 startLocalPos;
    Vector3 lastPlayerPos;

    void Start()
    {
        startLocalPos = transform.localPosition;
        playerRoot = transform.parent; // Player
        lastPlayerPos = playerRoot.position;
    }

    void Update()
    {
        bool isIdle = IsPlayerIdle();

        if (!isIdle)
        {
            // Reset position when moving
            transform.localPosition = startLocalPos;
            return;
        }

        float pixelSize = 1f / pixelsPerUnit;

        float yOffset =
            Mathf.Sin(Time.time * frequency * Mathf.PI * 2f)
            * amplitude * pixelSize;

        // Snap pixel-perfect
        yOffset = Mathf.Round(yOffset / pixelSize) * pixelSize;

        transform.localPosition = startLocalPos + Vector3.up * yOffset;
    }

    bool IsPlayerIdle()
    {
        float distance = Vector3.Distance(playerRoot.position, lastPlayerPos);
        lastPlayerPos = playerRoot.position;

        return distance < idleThreshold;
    }
}


