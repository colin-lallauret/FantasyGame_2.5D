using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float groundDist = 1f;

    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // --- Coller le joueur au sol ---
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up;

        if (Physics.Raycast(castPos, Vector3.down, out hit, Mathf.Infinity, terrainLayer))
        {
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y + groundDist;
            transform.position = newPos;
        }

        // --- Déplacement ---
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        Vector3 moveDir = new Vector3(x, 0f, y).normalized;

        rb.linearVelocity = moveDir * speed;

        // --- Flip du sprite ---
        if (x < 0) sr.flipX = true;
        if (x > 0) sr.flipX = false;
    }
}
