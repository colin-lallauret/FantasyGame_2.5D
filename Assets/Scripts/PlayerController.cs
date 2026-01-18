using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    [Header("Mouvement")]
    public float speed = 5f;
    public float groundDist = 0.2f; 

    [Header("Saut")]
    public float jumpForce = 8f; 
    private bool isGrounded = true;
    private bool jumping = false;
    private bool wasGroundedLastFrame = true;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;      
    public float dashDuration = 0.2f;  
    public float dashCooldown = 1f;    
    private bool isDashing = false;
    private bool canDash = true;

    [Header("Animation Procédurale")]
    public float idlePulseSpeed = 2f; 
    public float idlePulseAmount = 0.05f;
    public float tiltAmount = 10f;       
    public float squashStretchTime = 0.1f; 
    private Vector3 originalScale;

    [Header("Audio")]
    public AudioSource audioSource;    
    public AudioClip dashSound;       
    public AudioClip jumpSound;        

    [Header("Références")]
    public LayerMask terrainLayer;
    public Rigidbody rb;
    public SpriteRenderer sr;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalScale = transform.localScale;
        
        rb.isKinematic = false; 
        rb.useGravity = true;
        rb.constraints = RigidbodyConstraints.FreezeRotation; 
    }

    private void Update()
    {
        if (isDashing) return;

        // --- Détection du Sol ---
        RaycastHit hit;
        Vector3 castPos = transform.position + Vector3.up * 0.1f; 
        isGrounded = Physics.Raycast(castPos, Vector3.down, out hit, groundDist + 0.3f, terrainLayer);

        // --- Effet Atterrissage ---
        if (isGrounded && !wasGroundedLastFrame)
        {
            StartCoroutine(SquashRoutine(0.8f, 1.2f));
        }
        wasGroundedLastFrame = isGrounded;

        // --- Logique du Saut ---
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            Jump();
        }

        if (isGrounded && rb.linearVelocity.y <= 0.1f)
        {
            jumping = false;
            Vector3 newPos = transform.position;
            newPos.y = hit.point.y + groundDist;
            transform.position = newPos;
        }

        // --- Déplacement (FORÇAGE ZQSD / WASD REPRIS) ---
        float x = 0f;
        float y = 0f;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.W)) y = 1f;
        if (Input.GetKey(KeyCode.S)) y = -1f;
        if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.A)) x = -1f;
        if (Input.GetKey(KeyCode.D)) x = 1f;

        Vector3 moveDir = new Vector3(x, 0f, y).normalized;

        // --- Animations ---
        HandleAnimations(moveDir);

        // --- Dash ---
        if ((Input.GetKeyDown(KeyCode.LeftAlt) || Input.GetKeyDown(KeyCode.RightAlt)) && canDash && moveDir.magnitude > 0)
        {
            StartCoroutine(DashCoroutine(moveDir));
        }

        if (!isDashing)
        {
            Vector3 vel = moveDir * speed;
            vel.y = rb.linearVelocity.y; 
            rb.linearVelocity = vel;
        }

        if (x < 0) sr.flipX = true;
        if (x > 0) sr.flipX = false;
    }

    void HandleAnimations(Vector3 moveDir)
    {
        // Si elle ne bouge pas, qu'elle est au sol et ne saute pas
        if (moveDir.magnitude < 0.1f && isGrounded && !jumping)
        {
            // --- EFFET DE RESPIRATION (MONTER/DESCENDRE) ---
            // On calcule une valeur qui monte et descend avec le temps
            float breathing = Mathf.Sin(Time.time * idlePulseSpeed); 
            
            // 1. Elle "gonfle" (taille)
            float pulse = breathing * idlePulseAmount;
            transform.localScale = new Vector3(originalScale.x, originalScale.y + pulse, originalScale.z);
            
            // 2. Elle "monte et descend" (position)
            // On ajoute un tout petit mouvement vertical à sa position locale
            float verticalOffset = breathing * 0.02f; // Ajuste 0.02f pour l'amplitude du mouvement
            // Note : On ne change pas transform.position directement pour ne pas casser le collage au sol
            sr.transform.localPosition = new Vector3(0, verticalOffset, 0);

            transform.localRotation = Quaternion.identity;
        }
        else
        {
            // Quand elle bouge, on remet tout à zéro
            transform.localScale = originalScale;
            sr.transform.localPosition = Vector3.zero;
            
            // On garde l'inclinaison (Tilt) pour la course
            if (moveDir.magnitude >= 0.1f && isGrounded)
            {
                float tilt = -moveDir.x * tiltAmount; 
                transform.localRotation = Quaternion.Euler(0, 0, tilt);
            }
        }
    }

    void Jump()
    {
        jumping = true;
        StartCoroutine(SquashRoutine(1.3f, 0.7f));
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
        if (audioSource != null && jumpSound != null) audioSource.PlayOneShot(jumpSound);
    }

    private IEnumerator SquashRoutine(float targetY, float targetX)
    {
        Vector3 targetScale = new Vector3(originalScale.x * targetX, originalScale.y * targetY, originalScale.z);
        transform.localScale = targetScale;
        yield return new WaitForSeconds(squashStretchTime);
        transform.localScale = originalScale;
    }

    private IEnumerator DashCoroutine(Vector3 direction)
    {
        canDash = false;
        isDashing = true;
        if (audioSource != null && dashSound != null) audioSource.PlayOneShot(dashSound);

        rb.linearVelocity = direction * dashSpeed;
        yield return new WaitForSeconds(dashDuration);
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}