using UnityEngine;

public class KeishaIdleFloat : MonoBehaviour
{
    [Header("Idle Float Settings")]
    public float amplitude = 0.03f;
    public float frequency = 2f;
    public float movementThreshold = 0.0001f;

    private Vector3 startLocalPosition;
    private Vector3 lastWorldPosition;
    private float timer;

    void Start()
    {
        startLocalPosition = transform.localPosition;
        lastWorldPosition = transform.parent.position;
    }

    void Update()
    {
        Vector3 currentWorldPosition = transform.parent.position;

        bool isIdle = Vector3.Distance(currentWorldPosition, lastWorldPosition) < movementThreshold;

        if (isIdle)
        {
            timer += Time.deltaTime;
            float offsetY = Mathf.Sin(timer * frequency) * amplitude;

            transform.localPosition = new Vector3(
                startLocalPosition.x,
                startLocalPosition.y + offsetY,
                startLocalPosition.z
            );
        }
        else
        {
            timer = 0f;
            transform.localPosition = startLocalPosition;
        }

        lastWorldPosition = currentWorldPosition;
    }
}
