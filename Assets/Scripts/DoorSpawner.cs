using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [Header("Door Objects")]
    public GameObject doorClose;
    public GameObject doorOpen;

    [Header("New Area Objects")]
    public GameObject obstaclesToSpawn; 

    [Header("Player Inventory")]
    public PlayerInventory inventory;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip wrongSound;
    public AudioClip deletionSound;

    [Header("Camera Shake Settings")]
    public float shakeAmplitude = 2f;
    public float shakeFrequency = 1.5f;
    public float shakeDuration = 0.5f;

    [Header("Enemy Spawn")]
    public GameObject enemyPrefab;
    public Transform[] spawnPoints;
    public int enemiesPerPoint = 1;

    private bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isOpened) return;

        if (inventory != null && inventory.HasKey)
        {
            OpenDoor();
            SpawnEnemies();
        }
        else
        {
            PlayWrongSound();
        }
    }

    void OpenDoor()
    {
        isOpened = true;

        if (doorClose != null) doorClose.SetActive(false);
        if (doorOpen != null) doorOpen.SetActive(true);

        // ✅ Joue le son de la porte
        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);

        // ✅ APPEL DE TON SCRIPT SHAKE
        if (CinemachineShake.instance != null)
        {
            CinemachineShake.instance.TriggerShake(shakeAmplitude, shakeFrequency, shakeDuration);
        }

        DeleteSpawnedObstacles();
    }

    public void DeleteSpawnedObstacles()
    {
        if (obstaclesToSpawn != null)
        {
            if (audioSource != null && deletionSound != null)
            {
                audioSource.PlayOneShot(deletionSound);
            }

            Destroy(obstaclesToSpawn);
            Debug.Log("Porte ouverte : Shake Custom + Destruction.");
        }
    }

    void PlayWrongSound()
    {
        if (audioSource != null && wrongSound != null)
            audioSource.PlayOneShot(wrongSound);
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0) return;

        foreach (Transform point in spawnPoints)
        {
            for (int i = 0; i < enemiesPerPoint; i++)
            {
                GameObject enemy = Instantiate(enemyPrefab, point.position, point.rotation);
                if (enemy.CompareTag("Enemy") && EnemyManager.instance != null)
                {
                    EnemyManager.instance.RegisterEnemy();
                }
            }
        }
    }
}