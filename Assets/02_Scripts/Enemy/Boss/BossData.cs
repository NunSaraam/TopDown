using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossData : MonoBehaviour
{
    [SerializeField] private BossSO bossSO;

    private Animator animator;

    private int currentHealth;

    void Start()
    {
        currentHealth = bossSO.bossHealth;
    }

    void Update()
    {
        
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        if (currentHealth <= 0)
        {
            animator.SetTrigger("Die");
        }
    }
}
