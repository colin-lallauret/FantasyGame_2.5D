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

    public void RegisterEnemy()
    {
        enemiesAlive++;
        Debug.Log("Enemy REGISTERED. Alive = " + enemiesAlive);
    }

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

        // 🎵 Coupe musique + joue reward sound
        if (MusicManager.instance != null)
        {
            MusicManager.instance.StopMusic();
            MusicManager.instance.PlayRewardSound();
        }

        // 🎁 récompense
        if (rewardObject != null)
            rewardObject.SetActive(true);
    }
}
