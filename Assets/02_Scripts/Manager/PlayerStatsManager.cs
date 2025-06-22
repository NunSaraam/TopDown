using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance {  get; private set; }

    public int deathCount = 0;
    public int baseMaxHealth = 5;
    public int currentMaxHealth => baseMaxHealth + deathCount;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { 
            Destroy(gameObject); 
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterDeath()
    {
        deathCount++;
        SaveManager.Instance.SaveStats(deathCount);
    }

    public void LoadData()
    {
        PlaterStatsData data = SaveManager.Instance.LoadStats();
        deathCount = data.deathCount;
    }
}
