using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossController : MonoBehaviour
{
    public enum BossSkillType { Jump, Summon, Charge}

    [SerializeField] BossSO bossSO;
    [SerializeField] BossDropManager dropManager;

    [Header("회차 설정")]
    public int unlockJumpSkillAtLoop = 2;
    public int unlockSummonSkillAtLoop = 3;
    public int unlockChargeSkillAtLoop = 4;

    [Header("스킬 쿨타임")]
    public float jumpSkillCooldown = 15f;
    public float summonSkillCooldown = 45f;
    public float chargeSkillCooldown = 25f;

    [SerializeField] private List<WeaponSO> dropPerLoop;
    private float jumpTimer;
    private float summonTimer = 0f;
    private float chargeTimer;

    private bool isCasting = false;
    public bool isActive = false;

    private float bossmoveSpeed;
    [SerializeField] private int currentHealth;

    [SerializeField] private Slider healthSlider;
    [SerializeField] private Canvas healthCanvas;
    [SerializeField] private Vector3 canvasOffset = new Vector3(0, 2f, 0);

    private Transform player;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] Transform[] spawnPoints;
    [SerializeField] GameObject minionPrefab;
    [SerializeField] GameObject shadowPrefab;
    [SerializeField] GameObject fireTrailPrefab;
    [SerializeField] LayerMask playerMask;

    private bool isDead = false;
    private int currentLoop;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentLoop = PlayerStatsManager.Instance.loopCount;
        rb = GetComponent<Rigidbody2D>();

        currentHealth = bossSO.bossHealth;
        bossmoveSpeed = bossSO.moveSpeed;

        if (healthSlider != null)
        {
            healthSlider.maxValue = currentHealth;
            healthSlider.value = currentHealth;
        }

        healthCanvas.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (!isActive || isCasting) return;
        rb.velocity = moveDirection * bossmoveSpeed;
    }

    private void Update()
    {
        if (healthCanvas != null)
        {
            healthCanvas.transform.position = transform.position + canvasOffset;
            healthCanvas.transform.rotation = Quaternion.identity;
        }

        jumpTimer += Time.deltaTime;
        summonTimer += Time.deltaTime;
        chargeTimer += Time.deltaTime;

        if (currentLoop >= unlockJumpSkillAtLoop && jumpTimer >= jumpSkillCooldown)
        {
            UseJumpSkill();
            jumpTimer = 0;
        }


        if (!isCasting && currentLoop >= 3 && summonTimer >= summonSkillCooldown)
        {
            StartCoroutine(UseSummonSkill());
            summonTimer = 0f;
        }

        if (currentLoop >= unlockChargeSkillAtLoop && chargeTimer >= chargeSkillCooldown)
        {
            UseChargeSkill();
            chargeTimer = 0;
        }

        TrackPlayer();
    }

    public void ActivateBossAI()
    {
        isActive = true;
        if (healthCanvas != null)
            healthCanvas.gameObject.SetActive(true);
    }

    void TrackPlayer()
    {
        if (!isActive || isCasting || player == null)
        {
            moveDirection = Vector2.zero;
            return;
        }

        moveDirection = (player.position - transform.position).normalized;
    }

    void UseJumpSkill()
    {
        Vector3 playerPos = player.position;
        float maxJumpRange = 5f;

        Vector3 direction = (playerPos - transform.position).normalized;
        float distance = Vector3.Distance(transform.position, playerPos);


        float clampedDistance = Mathf.Min(distance, maxJumpRange);
        Vector3 jumpTarget = transform.position + direction * clampedDistance;


        GameObject shadow = Instantiate(shadowPrefab, jumpTarget, Quaternion.identity);
        StartCoroutine(JumpAttackRoutine(jumpTarget, shadow));
    }

    void UseChargeSkill()
    {
        StartCoroutine(ChargeRoutine());
    }



    void OnBossDefeated()
    {
        isActive = false;
        isCasting = false;
        StopAllCoroutines();

        rb.velocity = Vector2.zero;



        PlayerStatsManager.Instance.RegisterVictory();

        int loop = PlayerStatsManager.Instance.loopCount;
        int index = Mathf.Clamp(loop - 2, 0, dropPerLoop.Count - 1);

        WeaponSO dropWeapon = dropPerLoop[index];
        WeaponInventoryManager.Instance.UnlockWeapon(dropWeapon);

        if (dropManager != null)
        {
            dropManager.DropWeaponForCurrentLoop();
        }
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);

        if (healthSlider != null)
            healthSlider.value = currentHealth;

        if (currentHealth <= 0)
        {
            isDead = true;
            isActive = false;
            StopAllCoroutines();

            if (healthCanvas != null)
                healthCanvas.gameObject.SetActive(false);

            OnBossDefeated();
            Destroy(gameObject, 0.2f);
        }
    }

    IEnumerator JumpAttackRoutine(Vector3 targetPos, GameObject shadow)
    {
        isCasting = true;

        float jumpDelay = 1.5f;
        float damageRadius = 2f;

        yield return new WaitForSeconds(jumpDelay);

        transform.position = targetPos;

        Collider2D hit = Physics2D.OverlapCircle(targetPos, damageRadius, playerMask);
        if (hit != null)
        {
            var player = hit.GetComponent<PlayerController>();
            if (player != null)
            {
                player.Hit(2);
            }
        }

        isCasting = false;
        Destroy(shadow);
    }

    IEnumerator UseSummonSkill()
    {
        isCasting = true;

        // 연출 시간 (ex: 1초 대기)
        yield return new WaitForSeconds(1f);

        foreach (Transform spawnPoint in spawnPoints)
        {
            Instantiate(minionPrefab, spawnPoint.position, Quaternion.identity);
        }

        isCasting = false;
    }


    IEnumerator ChargeRoutine()
    {
        isCasting = true;

        float chargeDuration = 1.5f;
        float chargeSpeed = 10f;
        float fireTrailInterval = 0.2f;
        float elapsed = 0f;

        Vector3 direction = (player.position - transform.position).normalized;

        while (elapsed < chargeDuration)
        {
            transform.position += direction * chargeSpeed * Time.deltaTime;

            Instantiate(fireTrailPrefab, transform.position, Quaternion.identity);
            
            elapsed += fireTrailInterval;
            yield return new WaitForSeconds(chargeDuration);
        }

        isCasting = false;
    }
}
