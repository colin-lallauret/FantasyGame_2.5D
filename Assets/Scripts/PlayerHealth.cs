using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Hearts")]
    public int maxHearts = 3;
    public int currentHearts;

    public Image[] heartImages;      // Heart1, Heart2, Heart3
    public Sprite fullHeartSprite;   // Cœur plein
    public Sprite emptyHeartSprite;  // Cœur vide

    [Header("Death UI")]
    public GameObject deathPanel;    // DeathPanel (désactivé par défaut)

    private bool isDead = false;

    void Start()
    {
        currentHearts = maxHearts;
        UpdateHeartsUI();

        // Assure que le DeathPanel est caché au début
        if (deathPanel != null)
            deathPanel.SetActive(false);

        Time.timeScale = 1f; // Sécurité : remet le temps normal
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHearts -= amount;
        if (currentHearts < 0)
            currentHearts = 0;

        UpdateHeartsUI();

        if (currentHearts <= 0)
        {
            Die();
        }
    }

    void UpdateHeartsUI()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            if (heartImages[i] == null) continue;

            // Si i < points de vie → cœur plein
            if (i < currentHearts)
                heartImages[i].sprite = fullHeartSprite;
            else
                heartImages[i].sprite = emptyHeartSprite;
        }
    }

    void Die()
    {
        isDead = true;

        // Désactiver le mouvement
        var move = GetComponent<PlayerController>();
        if (move != null) move.enabled = false;

        // Désactiver le slash
        var slash = GetComponent<PlayerSlash>();
        if (slash != null) slash.enabled = false;

        // Afficher le panneau de mort
        if (deathPanel != null)
            deathPanel.SetActive(true);

        // Mettre le jeu en pause
        Time.timeScale = 0f;
    }

    // Appelé par le bouton Replay
    public void Replay()
    {
        Time.timeScale = 1f; // remettre le temps
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void Update()
    {
        // 🔧 TEST DEBUG : appuie sur H pour perdre 1 cœur
        // (Tu peux le retirer quand tu n’en as plus besoin)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(1);
        }
    }
}
