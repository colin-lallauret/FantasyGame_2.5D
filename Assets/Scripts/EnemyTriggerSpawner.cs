using UnityEngine;
using System.Collections;

public class EnemyTriggerSpawner : MonoBehaviour
{
    [Header("Enemy Spawn")]
    public GameObject enemyPrefab;      
    public Transform[] spawnPoints;     
    public int enemiesPerPoint = 1;

    [Header("Options")]
    public bool spawnOnlyOnce = true;
    public float spawnDelay = 2f;   // ⏱️ délai avant apparition (modifiable)

    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (spawnOnlyOnce && hasSpawned)
            return;

        hasSpawned = true;
        StartCoroutine(SpawnWithDelay());
    }

    IEnumerator SpawnWithDelay()
    {
        Debug.Log($"⏳ EnemyTriggerSpawner : Attente de {spawnDelay} secondes avant spawn...");

        // Attendre X secondes
        yield return new WaitForSeconds(spawnDelay);

        // Maintenant spawn !
        SpawnEnemies();
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null || spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemyTriggerSpawner : enemyPrefab ou spawnPoints non assignés.");
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            for (int i = 0; i < enemiesPerPoint; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);
                Debug.Log("Enemy SPAWNED : " + enemy.name);

                // Enregistrer dans EnemyManager
                if (EnemyManager.instance != null)
                    EnemyManager.instance.RegisterEnemy();
            }
        }

        Debug.Log("EnemyTriggerSpawner : Enemies spawned !");
    }
}
