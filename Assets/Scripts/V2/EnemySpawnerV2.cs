using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerV2 : MonoBehaviour
{
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] private PlayerV2 player;

    private float screenWidth;
    private const float MIN_SPAWN_DELAY = 2f;
    private const float MAX_SPAWN_DELAY = 4f;

    private void Start()
    {
        Initialize();
        StartCoroutine(SpawnEnemies());
    }

    private void Initialize()
    {
        player = FindObjectOfType<PlayerV2>();
        CalculateScreenWidth();
    }

    private void CalculateScreenWidth()
    {
        if (Camera.main != null)
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            screenWidth = screenBounds.x;
        }
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(Random.Range(MIN_SPAWN_DELAY, MAX_SPAWN_DELAY));
        }
    }

    private void SpawnEnemy()
    {
        Vector2 spawnableArea = player.transform.position;

        if (enemies.Count == 0) return;

        int randSpawnIndex = Random.Range(0, enemies.Count);

        Vector2 randomOffset = Random.insideUnitCircle.normalized * Random.Range(5f, 8f);
        Vector2 spawnPoint = spawnableArea + randomOffset;

        Instantiate(enemies[randSpawnIndex], spawnPoint, Quaternion.identity);
    }
}