using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    public MonsterSO monsterSO;

    public int currentHealth;

    private void Start()
    {
        currentHealth = monsterSO.monsterHealth;
    }

    public int GetAttackDamage()
    {
        return monsterSO.attackDamage;
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
        if (monsterSO.HealthItemPrefab != null)
        {
            Instantiate(monsterSO.HealthItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }

}
