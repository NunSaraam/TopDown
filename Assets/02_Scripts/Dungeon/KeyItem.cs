using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyItem : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            PlayerInventory.Instance.hasBossKey = true;
            Debug.Log(" ���踦 ȹ��");
            Destroy(gameObject);
        }
    }
}
