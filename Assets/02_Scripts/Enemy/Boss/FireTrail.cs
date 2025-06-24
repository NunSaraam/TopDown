using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTrail : MonoBehaviour
{
    public int damage = 1;
    public float duration = 5f;

    private void Start()
    {
        Destroy(gameObject, duration);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerController pc = collision.GetComponent<PlayerController>();
            if (pc != null)
            {
                pc.Hit(damage);
            }
        }
    }
}
