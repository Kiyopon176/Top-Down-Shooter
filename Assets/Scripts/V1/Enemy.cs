using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Enemy: MonoBehaviour
{
    [SerializeField] private int hp; 
    [SerializeField] private int damage = 10; 
    [SerializeField] private float speed = -0.1f; 
    [SerializeField] private EnemyAnimations enemyAnimations;
    [SerializeField] private lootItem[] lootTable;

    private float screenHeight;
    private Player player;
    private bool isMovable = true;

    private void Start()
    {
        if (Camera.main is not null)
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            screenHeight = screenBounds.y;
        }
        player = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        if (isMovable)
        {
            Movement();
        }
    }
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player"))
        {
            HandlePlayerCollision();
        }
        else if (other.collider.CompareTag("Bullet"))
        {
            HandleBulletCollision(other.gameObject);
        }
    }

    private void HandlePlayerCollision()
    {
        EventManager.OnDamaged?.Invoke(damage);
        Die();
        PlayDeathAnimation();
        DestroyEnemy();
    }

    private void HandleBulletCollision(GameObject bullet)
    {
        hp -= player.Damage;
        if (hp <= 0)
        {
            EventManager.OnEnemyKilled?.Invoke();
            isMovable = false;
            Die();
            PlayDeathAnimation();
            DestroyEnemy();
        }
        bullet.SetActive(false);
    }

    private void Die()
    {
        EventManager.OnSoundNeed?.Invoke("EnemyKilled");
        DropLoot();
    }
    
    private void DropLoot()
    {
        int totalChance = 0;

        foreach (var lootItem in lootTable)
        {
            totalChance += lootItem.dropChance;
        }

        int randomNumber = Random.Range(0, totalChance);

        int accumulatedChance = 0;

        foreach (var lootItem in lootTable)
        {
            accumulatedChance += lootItem.dropChance;

            if (randomNumber < accumulatedChance)
            {
                if (lootItem.itemPrefab != null)
                {
                    Instantiate(lootItem.itemPrefab, GetRandomPosition(), Quaternion.identity).SetActive(true);
                }
                break;
            }
        }
    }
    
    private Vector2 GetRandomPosition()
    {
        return new Vector2(transform.position.x + Random.Range(-1f, 1f), transform.position.y + Random.Range(-1f, 1f));
    }
    
    private void PlayDeathAnimation()
    {
        enemyAnimations.PlayDeathAnimation("IsDead");
    }

    private void DestroyEnemy()
    {
        Destroy(gameObject, enemyAnimations.enemyAnimLength);
    }

    private void Movement()
    {
        if(isMovable)
            transform.Translate(0, speed, 0);
        if (gameObject.transform.position.y < -screenHeight)
        {    
            print("outOfScreen");
            EventManager.OutOfScreen?.Invoke();
            Destroy(gameObject);
        }
    }
}
