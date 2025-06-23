using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("플레이어 체력UI")]
    public Sprite emptyHealth;
    public Sprite fullHealth;
    public Image[] healths;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject);
            return; 
        }
        Instance = this;
    }


    public void UpdataHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < healths.Length; i++)
        {
            if (i < currentHealth)
            {
                healths[i].sprite = fullHealth;
            }
            else
            {
                healths[i].sprite = emptyHealth;
            }

            healths[i].enabled = (i < maxHealth);
        }
    }

}
