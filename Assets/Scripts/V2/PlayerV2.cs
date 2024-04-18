using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerV2 : MonoBehaviour
{
    public static int Hp { get; private set; } = 100;
    public int Damage = 5;
    public bool UseJoystick = false;

    [SerializeField] private float speed = 3f;
    [SerializeField] private Joystick moveMent, rotation;
    [SerializeField] private EnemyAnimations enemyAnimations;
    [SerializeField] private Collider2D collider2D;

    private Rigidbody2D rb;
    private float screenHeight;
    private float screenWidth;

    private void Start()
    {
        InitializeComponents();
        CheckJoysticks();
        SetScreenBounds();
    }

    private void InitializeComponents()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void CheckJoysticks()
    {
        if (moveMent == null || rotation == null)
        {
            Debug.LogError("Joystick is not assigned!");
        }
    }

    private void SetScreenBounds()
    {
        if (Camera.main != null)
        {
            Vector3 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
            screenHeight = screenBounds.y;
            screenWidth = screenBounds.x;
        }
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
        HandleMovement();
        RotateSpriteWithJoystick();
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
        float horizontalInput = UseJoystick ? moveMent.Horizontal : Input.GetAxis("Horizontal");
        float verticalInput = UseJoystick ? moveMent.Vertical : Input.GetAxis("Vertical");
        return new Vector2(horizontalInput, verticalInput).normalized;
    }

    private void RotateSpriteWithJoystick()
    {
        float horizontal = rotation.Horizontal;
        float vertical = rotation.Vertical;
        float angle = Mathf.Atan2(horizontal, vertical) * Mathf.Rad2Deg;
        if (angle != 0)
        {
            rb.MoveRotation(-angle);
        }
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
                print(Damage);
                break;
        }
    }
}
