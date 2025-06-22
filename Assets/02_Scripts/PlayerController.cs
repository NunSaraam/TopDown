using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MonsterSO MonsterSO;
    [SerializeField] MonsterData monsterData;

    Animator animator;
    Rigidbody2D rb;

    public float moveSpeed = 10f;

    public int maxHealth;
    [SerializeField]public int currentHealth;

    bool isInvincible = false;
    public float invincibleDuration = 1f;

    bool isDead = false;

    private float moveHorizontal, moveVertical;
    Vector2 movement;
    private int facingDirection = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        if (PlayerStatsManager.Instance != null)
        {
            PlayerStatsManager.Instance.LoadData();
            maxHealth = PlayerStatsManager.Instance.currentMaxHealth;
        }
        else
        {
            Debug.LogWarning("PlayerStatsManager.Instance is null");
            maxHealth = 5;
        }

        currentHealth = maxHealth;
        
        if (UIManager.Instance != null)
        {
            UIManager.Instance.UpdataHealthUI(currentHealth, maxHealth);
        }
    }

    private void Update()
    {

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        movement = new Vector2 (moveHorizontal, moveVertical).normalized;

        animator.SetFloat("Velocity", movement.magnitude);

        if (movement.x != 0)
            facingDirection = movement.x > 0 ? 1 : -1;

        transform.localScale = new Vector2(facingDirection, 1);

        TestReStartStage();
    }

    private void FixedUpdate()
    {
        rb.velocity = movement * moveSpeed;
    }
        
    public void Hit(int damage)
    {
        if (isInvincible) return;

        currentHealth -= damage;

        animator.SetTrigger("Hit");
        
        UIManager.Instance.UpdataHealthUI(currentHealth, maxHealth);

        if (currentHealth <= 0)
        {
            Dead();
        }
        else
        {
            StartCoroutine(InvincibilityCoroutine());
        }
    }


    void Dead()
    {
        if (isDead) return;

        moveHorizontal = Input.GetAxisRaw("Horizontal");
        moveVertical = Input.GetAxisRaw("Vertical");

        PlayerStatsManager.Instance.RegisterDeath();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Hit(MonsterSO.damage);
        }

        if (collision.CompareTag("HealthItem"))
        {
            currentHealth = Mathf.Min(currentHealth + 1, maxHealth);
            if (UIManager.Instance != null)
            {
                UIManager.Instance.UpdataHealthUI(currentHealth, maxHealth);
            }
            Destroy(collision.gameObject);
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }


    void TestReStartStage()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
