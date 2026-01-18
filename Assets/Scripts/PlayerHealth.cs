using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    [Header("Hearts")]
    public int maxHearts = 3;
    public int currentHearts;
    public Image[] heartImages;      
    public Sprite fullHeartSprite;   
    public Sprite emptyHeartSprite;  

    [Header("Death UI")]
    public GameObject deathPanel;

    [Header("Audio")]
    public AudioSource audioSource;    
    public AudioClip hitSound;         
    public AudioClip deathSound;       

    [Header("Camera Shake (On Hit)")]
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2f;
    public float shakeDuration = 0.15f;

    [Header("Damage Flash")]
    public FlashDamageUI damageFlash;

    [Header("Knockback Settings")]
    public float knockbackForce = 10f;
    private bool isInvincible = false;
    private bool isDead = false;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();

        if (deathPanel != null)
            deathPanel.SetActive(false);

        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1f;
    }

    public void TakeDamage(int amount)
    {
        if (isDead || isInvincible) return;

        currentHearts -= amount;
        if (currentHearts < 0) currentHearts = 0;

        // Son de dégâts
        if (audioSource != null && hitSound != null)
            audioSource.PlayOneShot(hitSound);

        // Effets visuels
        if (CinemachineShake.instance != null)
            CinemachineShake.instance.TriggerShake(shakeAmplitude, shakeFrequency, shakeDuration);

        if (damageFlash != null) damageFlash.Flash();

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            StartCoroutine(DieRoutine());
        }
        else
        {
            StartCoroutine(KnockbackRoutine());
        }
    }

    private IEnumerator KnockbackRoutine()
    {
        isInvincible = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        PlayerController move = GetComponent<PlayerController>();

        if (rb != null)
        {
            if (move != null) move.enabled = false;
            rb.linearVelocity = Vector3.zero;
            rb.AddForce(-transform.forward * knockbackForce, ForceMode.Impulse);
        }

        yield return new WaitForSeconds(0.2f);
        if (move != null) move.enabled = true;

        yield return new WaitForSeconds(0.8f);
        isInvincible = false;
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;
            heartImages[i].sprite = (i < currentHearts) ? fullHeartSprite : emptyHeartSprite;
        }
    }

    private IEnumerator DieRoutine()
    {
        if (isDead) yield break;
        isDead = true;

        // ✅ APPEL CORRIGÉ : On utilise l'instance et la fonction StopAllMusic
        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopAllMusic();
        }

        // Joue le son de mort de Keisha
        if (audioSource != null && deathSound != null)
            audioSource.PlayOneShot(deathSound);

        // Désactive les mouvements et le combat
        if (GetComponent<PlayerController>() != null) GetComponent<PlayerController>().enabled = false;
        if (GetComponent<PlayerSlash>() != null) GetComponent<PlayerSlash>().enabled = false;

        // Attente pour entendre le cri de mort (1.5s)
        yield return new WaitForSecondsRealtime(1.5f);

        if (deathPanel != null) deathPanel.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) TakeDamage(1);
    }
}