using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MonsterSO MonsterSO;
    [SerializeField] MonsterData monsterData;

    Animator animator;
    Rigidbody2D rb;

    public float moveSpeed = 10f;

    public int maxHealth = 10;
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
        currentHealth = maxHealth;
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
        isDead = true;

        rb.velocity = Vector2.zero;
        rb.isKinematic = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Hit(MonsterSO.damage);
        }

        if (collision.CompareTag("HealthItem"))
        {
            currentHealth++;
            Destroy(collision.gameObject);
        }
    }

    IEnumerator InvincibilityCoroutine()
    {
        isInvincible = true;

        yield return new WaitForSeconds(invincibleDuration);

        isInvincible = false;
    }
}
