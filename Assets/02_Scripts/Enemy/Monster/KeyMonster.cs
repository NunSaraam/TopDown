using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyMonster : MonoBehaviour
{
    [SerializeField] private float detectRange = 10f;
    [SerializeField] private MonsterSO monsterSO;

    public float wallDectDistance = 0.5f;
    public LayerMask obstacleLayer;

    private Transform player;
    private Rigidbody2D rb;
    private Animator animator;

    public GameObject keyPrefab;
    
    private bool isRunning = false;
    
    private int currentHealth;
    private Vector2 currentDirection;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        currentHealth = monsterSO.monsterHealth;
    }

    private void Update()
    {
        if (player == null) return;
        
        float distance = Vector2.Distance(transform.position, player.position);


        isRunning = distance <= detectRange;
        animator.SetBool("IsRunning", isRunning);
    }

    private void FixedUpdate()
    {
        if (isRunning)
        {
            Vector2 directionToPlayer = (transform.position - player.position).normalized;
            if (currentDirection == Vector2.zero)
            {
                currentDirection = directionToPlayer;
            }

            if (IsObstacleAhead(currentDirection))
            {
                currentDirection = Quaternion.Euler(0, 0, Random.Range(135f, 225f)) * currentDirection;
            }

            rb.velocity = currentDirection * monsterSO.moveSpeed;

            if (currentDirection.x != 0)
            {
                transform.localScale = new Vector3(Mathf.Sign(currentDirection.x), 1, 1);
            }
        }
        else
        {
            rb.velocity = Vector2.zero;
            currentDirection = Vector2.zero;
        }
    }
    private bool IsObstacleAhead(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, wallDectDistance, obstacleLayer);
        return hit.collider != null;
    }

    private void OnDrawGizmosSelected()
    {
        if (Application.isPlaying && currentDirection != Vector2.zero)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, currentDirection * wallDectDistance);
        }

        Gizmos.color= Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Instantiate(keyPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
