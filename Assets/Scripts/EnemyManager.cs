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
        // Sécurité : cacher la reward au début
        if (rewardObject != null)
            rewardObject.SetActive(false);
    }

    // ✅ On passe l'objet spawn, et on compte UNIQUEMENT les tag "Enemy"
    public void RegisterEnemy(GameObject spawned)
    {
        if (spawned == null) return;

        if (!spawned.CompareTag("Enemy"))
        {
            Debug.Log($"[EnemyManager] IGNORE Register : {spawned.name} (Tag={spawned.tag})");
            return;
        }

        enemiesAlive++;
        Debug.Log("Enemy REGISTERED. Alive = " + enemiesAlive);
    }

    // ✅ Pareil : on décrémente UNIQUEMENT pour les "Enemy"
    public void EnemyDied(GameObject killed)
    {
        if (killed == null) return;

        if (!killed.CompareTag("Enemy"))
        {
            Debug.Log($"[EnemyManager] IGNORE Died : {killed.name} (Tag={killed.tag})");
            return;
        }

        enemiesAlive--;
        if (enemiesAlive < 0) enemiesAlive = 0;

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

        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
            MusicManager.instance.PlayRewardSound();
        }

        if (rewardObject != null)
            rewardObject.SetActive(true);
    }
}
