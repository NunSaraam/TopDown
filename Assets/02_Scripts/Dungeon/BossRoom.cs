using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoom : MonoBehaviour
{
    [SerializeField] BossController boss;

    private bool playerInRange = false;

    public GameObject door;

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (PlayerInventory.Instance.hasBossKey)
            {
                door.SetActive(false);
                boss.ActivateBossAI();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        { 
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
            playerInRange = false;
    }
}
