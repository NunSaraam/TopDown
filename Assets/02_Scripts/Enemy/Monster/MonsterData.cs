using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    [SerializeField] MonsterSO monsterSO;
    PlayerController playerController;

    public int currentHealth;

    private void Start()
    {
        currentHealth = monsterSO.monsterHealth;
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void DealDamage()
    {
        playerController.Hit(monsterSO.damage);
    }

    void Die()
    {
        if (monsterSO.HealthItemPrefab != null)
        {
            Instantiate(monsterSO.HealthItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
