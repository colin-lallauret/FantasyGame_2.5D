using UnityEngine;
using System.Collections;

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

    [Header("Wave Settings")]
    public int totalWaves = 3;
    public float waveDelay = 2f;  // délai entre chaque vague

    private bool isOpened = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || isOpened) return;

        if (inventory != null && inventory.HasKey)
        {
            OpenDoor();
            StartCoroutine(SpawnWaves());  // 👈 lancement des vagues !
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

    // 👇 Coroutine : spawn en 3 vagues avec délai
    IEnumerator SpawnWaves()
    {
        for (int wave = 1; wave <= totalWaves; wave++)
        {
            Debug.Log($"🌊 Vague {wave}/{totalWaves} lancée !");
            SpawnEnemies(); // spawn d’une vague

            yield return new WaitForSeconds(waveDelay); 
        }

        Debug.Log("🔥 Toutes les vagues ont été spawn !");
    }

    void SpawnEnemies()
    {
        if (enemyPrefab == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("DoorSpawner : enemyPrefab ou spawnPoints non assignés.");
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            for (int i = 0; i < enemiesPerPoint; i++)
            {
                Instantiate(enemyPrefab, point.position, point.rotation);
            }
        }
    }
}
