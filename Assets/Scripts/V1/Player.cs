using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    public int Hp = 100;
    public int Damage = 5;
    public bool UseJoystick = false;

    [SerializeField] private float speed = 3f;
    [SerializeField] private Joystick moveMent;
    [SerializeField] private EnemyAnimations enemyAnimations;

    private Rigidbody2D rb;
    private float screenHeight;
    private float screenWidth;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        if (moveMent == null)
        {
            Debug.LogError("Joystick is not assigned!");
        }
        Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        screenHeight = screenBounds.y;
        screenWidth = screenBounds.x;
    }

    private void OnEnable()
    {
        EventManager.OnDamaged += HpChange;
    }

    private void HpChange(int difference)
    {
        Hp += difference;
        EventManager.OnHpChanged?.Invoke();

        if (Hp <= 0)
        {
            StartCoroutine(DeathCoroutine());
        }
    }

    private IEnumerator DeathCoroutine()
    {
        enemyAnimations.PlayDeathAnimation("IsDead");
        yield return new WaitForSeconds(enemyAnimations.playerAnimLength);
        Time.timeScale = 0;
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        var position = transform.position;
        float clampedX = Mathf.Clamp(position.x, -screenWidth + 0.5f, screenWidth - 0.5f);
        float clampedY = Mathf.Clamp(position.y, -screenHeight + 0.5f, screenHeight - 0.5f);

        position = new Vector3(clampedX, clampedY, position.z);
        transform.position = position;
        
        HandleMovement();
    }

    private void HandleMovement()
    {
        Vector2 movementDirection = GetMovementInput();
        if (movementDirection != Vector2.zero)
        {
            rb.MovePosition(rb.position + movementDirection * (speed * Time.fixedDeltaTime));
        }
    }

    private Vector2 GetMovementInput()
    {
        float horizontalInput;
        if (UseJoystick)
        {
            horizontalInput = moveMent.Horizontal;
        }
        else
        {
            horizontalInput = Input.GetAxis("Horizontal");
            if (Input.GetKey(KeyCode.A))
            {
                horizontalInput = -1f;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                horizontalInput = 1f;
            }
        }
        
        float verticalInput = UseJoystick ? moveMent.Vertical : Input.GetAxis("Vertical");
        return new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        HandleCollision(other);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleCollision(collision.collider);
    }

    private void HandleCollision(Collider2D otherCollider)
    {
        if (otherCollider.CompareTag("Boost"))
        {
            HandleBoostCollision(otherCollider);
        }
    }

    private void HandleBoostCollision(Collider2D otherCollider)
    {
        lootItem lootItem = otherCollider.gameObject.GetComponent<lootItem>();
        if (lootItem != null)
        {
            ApplyBoost(lootItem);
        }
        EventManager.OnSoundNeed?.Invoke("Bonus");
        Destroy(otherCollider.gameObject);
    }

    private void ApplyBoost(lootItem lootItem)
    {
        switch (lootItem.boostName)
        {
            case "Heal":
                if (Hp < 100)
                {
                    Hp += 10;
                    EventManager.OnHpChanged?.Invoke();
                }
                break;
            case "PowerUp":
                Damage += 10;
                break;
        }
    }
}
