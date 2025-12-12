using UnityEngine;

public class DoorSpawner : MonoBehaviour
{
    [Header("Door Objects")]
    public GameObject doorClose;
    public GameObject doorOpen;

    [Header("Player Inventory")]
    public PlayerInventory inventory;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip successSound;
    public AudioClip wrongSound;

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

        if (audioSource != null && successSound != null)
            audioSource.PlayOneShot(successSound);
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

                // ✅ enregistrer l’ennemi dans EnemyManager
                if (EnemyManager.instance != null)
                    EnemyManager.instance.RegisterEnemy();
            }
        }
    }
}
