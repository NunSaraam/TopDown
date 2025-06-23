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


    public float detectRange = 6f;
    public float stopChaseRange = 8f;


    private enum MonsterState { Idle, Patrol, Chase, Charge}
    private MonsterState currentState = MonsterState.Patrol;

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

        UpdateState();
        HandleState();
    }
    void UpdateState()
    {
        float distance = Vector3.Distance(player.position, transform.position);

        switch (currentState)
        {
            case MonsterState.Patrol:
            case MonsterState.Idle:
                if (distance < detectRange)
                {
                    currentState = MonsterState.Chase;
                }
                break;

            case MonsterState.Chase:
                if (distance > stopChaseRange)
                {
                    currentState = MonsterState.Patrol;
                    patrolTarget = transform.position;
                }
                break;

            case MonsterState.Charge:
                break;
        }
    }

    void HandleState()
    {
        switch (currentState)
        {
            case MonsterState.Idle:
                break;

            case MonsterState.Patrol:
                HandlePatrol();
                break;

            case MonsterState.Chase:
                ChasePlayer();
                HandleSkill();
                break;

            case MonsterState.Charge:
                break;
        }
    }

    void ChasePlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
        FaceTarget(direction);
    }

    void HandlePatrol()
    {
        if (Vector3.Distance(transform.position, patrolTarget) < 0.5f)
        {
            patrolTarget = transform.position + (Vector3)(Random.insideUnitCircle * patrolRadius);
        }

        Vector3 direction = (patrolTarget - transform.position).normalized;
        transform.position += direction * monsterSO.moveSpeed * Time.deltaTime;
        FaceTarget(direction);
    }


    void HandleSkill()
    {
        if (monsterSO.canSkill) return;
        if (skillColldownTimer > 0f) return;
        
        skillColldownTimer = monsterSO.skillCooldown;
        StartCoroutine(ChargeAttack());
    }

    void FaceTarget(Vector3 direction)
    {
        if (direction.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }


    IEnumerator ChargeAttack()
    {
        currentState = MonsterState.Charge;

        animator.SetTrigger("Charge");

        Vector3 direction = (player.position - transform.position).normalized;
        FaceTarget(direction);

        float chargeDuration = 0.5f;
        float elapsed = 0f;

        while (elapsed < chargeDuration)
        {
            transform.position += direction * monsterSO.moveSpeed * chargeSpeedMultipilier * Time.deltaTime;
            elapsed += Time.deltaTime;
            yield return null;
        }

        currentState = MonsterState.Charge;
    }


}
