using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    private int enemiesAlive = 0;
    private bool rewardTriggered = false;

    [Header("Reward")]
    public GameObject rewardObject;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (rewardObject != null)
        {
            rewardObject.SetActive(false);
            Debug.Log("[EnemyManager] rewardObject désactivé au Start.");
        }
        else
        {
            Debug.LogWarning("[EnemyManager] rewardObject n'est PAS assigné !");
        }
    }

    // Appelé par LE spawner qui doit compter pour la reward
    public void RegisterEnemy()
    {
        enemiesAlive++;
        Debug.Log("Enemy REGISTERED. Alive = " + enemiesAlive);
    }

    // Appelé quand un enemy meurt (via SlashDamage)
    public void EnemyDied()
    {
        enemiesAlive--;
        Debug.Log("Enemy DIED. Alive = " + enemiesAlive);

        if (enemiesAlive <= 0 && !rewardTriggered)
        {
            rewardTriggered = true;
            OnAllEnemiesDead();
        }
    }

    void OnAllEnemiesDead()
    {
        Debug.Log("✅ ALL ENEMIES DEAD → Reward!");

        // 🎵 Couper musique + son reward
        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
            MusicManager.instance.PlayRewardSound();
        }

        // 🎁 Afficher le portail / reward
        if (rewardObject != null)
        {
            rewardObject.SetActive(true);
            Debug.Log("[EnemyManager] rewardObject.SetActive(true) appelé.");
        }
        else
        {
            Debug.LogWarning("[EnemyManager] rewardObject est NULL au moment du reward !");
        }
    }
}
