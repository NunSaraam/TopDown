using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonoBehaviour
{
    [SerializeField] MonsterSO monsterSO;
    [SerializeField] MonsterData monsterData;

    private Transform player;
    private Animator animator;

    private float attackCooldownTimer = 0f;
    private float skillColldownTimer = 0f;

    private Vector3 patrolTarget;
    private float patrolRadius = 3f;
    private float chargeSpeedMultipilier = 2f;

    private bool isCharging = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();

        if (animator.runtimeAnimatorController == null && monsterSO.animatorController != null)
        {
            animator.runtimeAnimatorController = monsterSO.animatorController;
        }

        patrolTarget = transform.position;
    }

    private void Update()
    {
        attackCooldownTimer -= Time.deltaTime;
        skillColldownTimer -= Time.deltaTime;
        
        if (!isCharging)
        {
            patrolOrChase();
        }

        HandleAttack();
        HandleSkill();
    }

    void patrolOrChase()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer < 5f)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
        }
        else
        {
            if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
            {
                patrolTarget = transform.position + (Vector3)(Random.insideUnitCircle * patrolRadius);
            }
            else
            {
                Vector3 direction = (patrolTarget - transform.position).normalized;
                transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
            }
        }
    }

    void HandleAttack()
    {
        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= monsterSO.attackRange && attackCooldownTimer < 0f)
        {
            attackCooldownTimer = monsterSO.attackCooldown;
            //animator.SetTrigger("Attack");

            // 플레이어 데미지 주기 추가
            if (attackCooldownTimer < 0f)
            {
                
            }
        }
    }

    void HandleSkill()
    {
        if (monsterSO.canSkill && skillColldownTimer <= 0f)
        {
            skillColldownTimer = monsterSO.skillCooldown;
            StartCoroutine(ChargeAttack());
        }
    }

    IEnumerator ChargeAttack()
    {
        isCharging = true;
        animator.SetTrigger("Charge");

        Vector3 direction = (player.position - transform.position).normalized;

        float chargeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < chargeDuration)
        {
            transform.position += direction * monsterSO.moveSpeed * chargeSpeedMultipilier * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        isCharging = false;
    }
}
