using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RangedMonster : MonoBehaviour
{
    [SerializeField] MonsterSO monsterSO;
    [SerializeField] MonsterData monsterData;

    public GameObject rangedAttackPrefab;
    public Transform attackPoint;

    private Transform player;
    private Animator animator;

    private float attackCooldownTimer = 0f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (monsterData == null )
            monsterData = GetComponent<MonsterData>();

        if (animator.runtimeAnimatorController == null && monsterSO.animatorController != null)
        {
            animator.runtimeAnimatorController = monsterSO.animatorController;
        }
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;

        PatrolOrKeepDistance();
        HandleAttack();
    }

    void PatrolOrKeepDistance()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if ( distanceToPlayer < 8f)
        {
            if (distanceToPlayer < monsterSO.attackRange)
            {
                Vector3 direction = (transform.position - player.position).normalized;
                transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
            }
        }
        else
        {
            transform.position += Vector3.left * monsterSO.moveSpeed * Time.deltaTime;
        }
    }

    void HandleAttack()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= monsterSO.attackRange && attackCooldownTimer <= 0f)
        {
            attackCooldownTimer = monsterSO.attackCooldown;
            animator.SetTrigger("Attack");

            Vector3 direction = (player.position - attackPoint.position).normalized;

            GameObject rangedAttack = Instantiate(rangedAttackPrefab, attackPoint.position, Quaternion.identity);
            Rigidbody2D rb = rangedAttack.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * 10f;
            }
        }
    }
}
