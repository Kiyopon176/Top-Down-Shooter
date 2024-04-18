using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private List<Enemy> enemies;

    private float screenWidth;

    void Start()
    {
        if (Camera.main is not null)
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            screenWidth = screenBounds.x;
        }
        StartCoroutine(nameof(EnemySpawn));
    }

    IEnumerator EnemySpawn()
    {
        while (true)
        {
            int randSpawnIndex = Random.Range(0, enemies.Count);
            Vector3 spawnPoint = new Vector3(Random.Range(-screenWidth, screenWidth), 7, 0);
            Instantiate(enemies[randSpawnIndex], spawnPoint, quaternion.identity);
            yield return new WaitForSeconds(2);
        }
    }
}