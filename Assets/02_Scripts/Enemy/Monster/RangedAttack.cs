using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttack : MonoBehaviour
{   
    public float attackLifeTimer = 4f;
    public int damage;

    void Start()
    {
        Destroy(gameObject, attackLifeTimer);

    }

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController controller = GetComponent<PlayerController>();
            if (controller != null)
            {
                controller.Hit(damage);
            }

            Destroy(gameObject);
        }

        /*
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
        */
    }
}
