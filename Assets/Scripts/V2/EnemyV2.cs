using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyV2 : MonoBehaviour
{
    [SerializeField] private int hp; 
    [SerializeField] private int damage = 10; 
    [SerializeField] private float speed = -0.1f; 
    [SerializeField] private EnemyAnimations enemyAnimations;
    [SerializeField] private lootItem[] lootTable;

    private PlayerV2 player;
    private bool isMovable = true;

    private void Start()
    {
        player = FindObjectOfType<PlayerV2>();
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
        foreach (var lootItem in lootTable)
        {
            if (Random.Range(0, 100) < lootItem.dropChance)
            {
                Instantiate(lootItem.itemPrefab, GetRandomPosition(), Quaternion.identity).SetActive(true);
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
        Vector3 direction = (player.transform.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;
        RotateTowardsPlayer(direction);
    }

    private void RotateTowardsPlayer(Vector3 direction)
    {
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
