using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonster : MonoBehaviour
{
    [SerializeField] MonsterSO monsterSO;
    [SerializeField] MonsterData monsterData;

    private Transform player;
    private Animator animator;

    private float skillColldownTimer = 0f;

    private Vector3 patrolTarget;
    private float patrolRadius = 3f;
    private float chargeSpeedMultipilier = 2f;

    private bool isCharging = false;
    private bool isChasing = false;

    public float detectRange = 6f;
    public float stopChaseRange = 8f;

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
        skillColldownTimer -= Time.deltaTime;

        UpdateChaseState();

        if (isCharging)
            return;

        if (isChasing)
        {
            chasePlayer();
        }
        else
        { 
            Handlepatrol();
        }
        HandleSkill();
    }

    void chasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
    }

    void Handlepatrol()
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


    void HandleSkill()
    {
        if (monsterSO.canSkill && skillColldownTimer <= 0f)
        {
            skillColldownTimer = monsterSO.skillCooldown;
            StartCoroutine(ChargeAttack());
        }
    }

    void UpdateChaseState()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        if (distance < detectRange)
        {
            isChasing = true;
        }
        else if ( distance > stopChaseRange)
        {
            isChasing = false;
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
