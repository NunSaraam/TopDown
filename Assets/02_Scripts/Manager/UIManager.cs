using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public PlayerController controller;

    [Header("플레이어 체력UI")]
    public int health;
    public int pMaxHealth;
    public Sprite emptyHealth;
    public Sprite fullHealth;
    public Image[] healths;

    void Start()
    {
        pMaxHealth = controller.maxHealth;
        health = controller.currentHealth;
    }

    void Update()
    {
        pMaxHealth = controller.maxHealth;
        health = controller.currentHealth;

        for (int i = 0; i < healths.Length; i++)
        {
            if (i < health)
            {
                healths[i].sprite = fullHealth;
            }
            else
            {
                healths[i].sprite = emptyHealth;
            }

            healths[i].enabled = (i < pMaxHealth);
        }

    }
}
