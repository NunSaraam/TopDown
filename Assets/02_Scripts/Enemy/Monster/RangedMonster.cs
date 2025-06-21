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
        stateTimer -= Time.deltaTime;

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
                bool goPatrol = Random.value < 0.5f;
                currentState = goPatrol ? State.Patrol : State.Idle;
                stateTimer = goPatrol ? patrolTime : idleTime;

                patrolDirection = Random.value < 0.5f ? Vector2.left : Vector2.right;
            }
        }

        switch (currentState)
        {
            //정지상태
            case State.Idle :   HandleIdle(); break;

            //정찰상태
            case State.Patrol:  HandlePatrol(); break;

            //도망상태
            case State.Flee:    HandlFlee(); break;

            //공격상태
            case State.Attack:  HandleAttack(); break;
        }
    }

    void HandleIdle()
    {
        if (stateTimer <= 0f)
        {
            currentState = State.Patrol;
            stateTimer = patrolTime;
            ChooseRandomPatrolDirection();
        }
    }

    void HandlePatrol()
    {
        transform.position += (Vector3)(patrolDirection * monsterSO.moveSpeed * Time.deltaTime);

        if (patrolDirection.x != 0)
            transform.localScale = new Vector3(Mathf.Sign(patrolDirection.x), 1, 1);


        if (stateTimer <= 0f)
        {
            currentState = State.Idle;
            stateTimer = idleTime;
        }
    }

    void HandlFlee()
    {
        Vector3 fleeDir = (transform.position - player.position).normalized;
        transform.position += fleeDir * monsterSO.moveSpeed * Time.deltaTime;
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

    void ChooseRandomPatrolDirection()
    {
        Vector2[] direction =
        {
            Vector2.left,
            Vector2.right,
            Vector2.up,
            Vector2.down
        };

        patrolDirection = direction[Random.Range(0, direction.Length)];
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall"))
        {
            ChooseRandomPatrolDirection();
        }
    }
}
