using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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

    [Header("Camera Shake (On Hit)")]
    public float shakeAmplitude = 1.2f;
    public float shakeFrequency = 2f;
    public float shakeDuration = 0.15f;

    [Header("Damage Flash")]
    public FlashDamageUI damageFlash;   // ✅ script sur l'image rouge

    private bool isDead = false;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();

        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f;
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHearts -= amount;
        if (currentHearts < 0)
            currentHearts = 0;

        // 📷 Camera shake à chaque hit
        if (CinemachineShake.instance != null)
        {
            CinemachineShake.instance.TriggerShake(
                shakeAmplitude,
                shakeFrequency,
                shakeDuration
            );
        }

        // 🟥 Flash rouge à l'écran
        if (damageFlash != null)
            damageFlash.Flash();

        UpdateHeartsUI();

        if (currentHearts <= 0)
            Die();
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            if (i < currentHearts)
                heartImages[i].sprite = fullHeartSprite;
            else
                heartImages[i].sprite = emptyHeartSprite;
        }
    }

    void Die()
    {
        isDead = true;

        var move = GetComponent<PlayerController>();
        if (move != null) move.enabled = false;

        var slash = GetComponent<PlayerSlash>();
        if (slash != null) slash.enabled = false;

        if (deathPanel != null)
            deathPanel.SetActive(true);

        Time.timeScale = 0f;
    }

    public void Replay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        // TEST DEBUG : appuie sur H pour perdre 1 cœur
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }
}
