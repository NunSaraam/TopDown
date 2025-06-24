using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsManager : MonoBehaviour
{
    public static PlayerStatsManager Instance {  get; private set; }

    public int deathCount = 0;
    public int baseMaxHealth = 5;

    public int loopCount = 1;

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

        LoadData();
    }

    public void RegisterDeath()
    {
        deathCount++;
        SaveManager.Instance.SaveStats(deathCount, loopCount);
    }

    public void LoadData()
    {
        PlaterStatsData data = SaveManager.Instance.LoadStats();
        deathCount = data.deathCount;
        loopCount = data.loopCount > 0 ? data.loopCount : 1;
    }
    public void RegisterVictory()
    {
        loopCount++;
        SaveManager.Instance.SaveStats(deathCount, loopCount);
        Debug.Log($"회차 증가: 현재 {loopCount}회차");
    }

    public void ResetStats()
    {
        deathCount = 0;
        loopCount = 1;
    }
}
