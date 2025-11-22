using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 3f;
    public float stopDistance = 1.2f;

    void Start()
    {
        // si pas assigné dans l'inspector → cherche le Player par tag
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null) player = p.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            transform.position += direction * speed * Time.deltaTime;
        }

        // pas de rotation
        transform.rotation = Quaternion.identity;
    }
}
