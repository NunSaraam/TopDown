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

    //몬스터 상태관리
    private float stateTimer = 0f;
    private enum State
    {
        Idle,
        Patrol,
        Flee,
        Attack
    }
    private State currentState = State.Patrol;
    private Vector2 patrolDirection = Vector2.left;

    [SerializeField] float idleTime = 2f;
    [SerializeField] float patrolTime = 3f;

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

        float distanceToPlayer = Vector3.Distance(player.position, transform.position);

        if (distanceToPlayer <= monsterSO.attackRange)
        {
            currentState = State.Attack;
        }
        else if (distanceToPlayer <= 4f)
        {
            currentState = State.Flee;
        }
        else
        {
            if (currentState != State.Patrol && currentState != State.Idle)
            {
                currentState = Random.value < 0.5f ? State.Patrol : State.Idle;
                stateTimer = currentState == State.Patrol ? patrolTime : idleTime;
                patrolDirection = Random.value < 0.5f ? Vector2.left : Vector2.right;
            }
        }
        stateTimer = Time.deltaTime;

        switch (currentState)
        {
            //정지상태
            case State.Idle:
                if (stateTimer <= 0f)
                {
                    currentState = State.Patrol;
                    stateTimer = patrolTime;
                    patrolDirection = Random.value < 0.5f ? Vector2.left : Vector2.right;
                }
                break;
            //정찰상태
            case State.Patrol:
                transform.position += (Vector3)(patrolDirection * monsterSO.moveSpeed * Time.deltaTime);
                if (patrolDirection.x < 0)
                    transform.localScale = new Vector3(-1, 1, 1);
                else
                    transform.localScale = new Vector3(1, 1, 1);

                if (stateTimer <= 0f)
                {
                    currentState = State.Idle;
                    stateTimer = idleTime;
                }
                break;
            //도망상태
            case State.Flee:
                Vector3 fleeDir = (transform.position - player.position).normalized;
                transform.position += fleeDir * monsterSO.moveSpeed * Time.deltaTime;
                break;
            //공격상태
            case State.Attack:
                HandleAttack();
                break;
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
                rb.gravityScale = 0f;
                rb.velocity = direction * 10f;
            }

            RangedAttack attack = rangedAttack.GetComponent<RangedAttack>();
            if ( attack != null)
            {
                attack.SetDamage(monsterSO.damage);
            }
        }
    }
}
