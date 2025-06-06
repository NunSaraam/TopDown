using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator animator;
    Rigidbody2D rb;

    public float moveSpeed = 10f;
    public int maxHealth = 10;
    public int currentHealth;

    bool isDead = false;

    private float moveHorizontal, moveVertical;
    Vector2 movement;
    private int facingDirection = 1;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
            Hit(10);


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
        
    void Hit(int damage)
    {
        animator.SetTrigger("Hit");
        currentHealth -= damage;
        
    }

    void Dead()
    {
        if (currentHealth <= 0)
        {
            isDead = true;

            rb.velocity = Vector2.zero;
            rb.isKinematic = true;
        }
    }
}
