using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] WeaponSO WeaponSO;

    public int gunDamage;
    public float bulletLifeTime = 2f;


    void Start()
    {
        gunDamage = WeaponSO.damage;
        
        Destroy(gameObject, bulletLifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            MonsterData monsterData = collision.GetComponent<MonsterData>();
            if (monsterData != null)
            {
                monsterData.TakeDamage(WeaponSO.damage);
            }

            Destroy(gameObject);
        }

        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}
