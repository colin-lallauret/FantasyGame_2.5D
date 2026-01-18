using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager instance;

    [Header("Progression")]
    public int enemiesAlive = 0;
    private bool bossSpawned = false;

    [Header("Boss Settings")]
    public GameObject bossPrefab;
    public Transform bossSpawnPoint;

    [Header("Portail")]
    public GameObject portal;

    void Awake() 
    { 
        instance = this; 
    }

    void Start()
    {
        if (portal != null) portal.SetActive(false);
    }

    public void RegisterEnemy() { enemiesAlive++; }

    public void EnemyDied()
    {
        enemiesAlive--;
        if (enemiesAlive <= 0 && !bossSpawned)
        {
            SpawnBoss();
        }
    }

    void SpawnBoss()
    {
        bossSpawned = true;
        if (bossPrefab != null && bossSpawnPoint != null)
        {
            Instantiate(bossPrefab, bossSpawnPoint.position, bossSpawnPoint.rotation);
            Debug.Log("Le Boss est apparu !");
        }
    }

    public void BossDied()
    {
        if (MusicManager.instance != null)
        {
            MusicManager.instance.StartVictorySequence();
        }
    }

    public void ActivatePortal()
    {
        if (portal != null)
        {
            portal.SetActive(true);
            Debug.Log("Portail activé !");
        }
    }
}