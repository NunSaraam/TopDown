using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class MonsterData : MonoBehaviour
{
    [SerializeField] MonsterSO data;

    public int currentHealth;

    private void Start()
    {
        currentHealth = data.monsterHealth;
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
        if (data.HealthItemPrefab != null)
        {
            Instantiate(data.HealthItemPrefab, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}
