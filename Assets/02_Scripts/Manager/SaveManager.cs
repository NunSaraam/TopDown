using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlaterStatsData
{
    public int deathCount;
}

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    private string path => Application.persistentDataPath + "/player_stats.json";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        { Destroy(gameObject); 
            return; 
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SaveStats(int deathCount)
    {
        PlaterStatsData data = new PlaterStatsData { deathCount = deathCount };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(path, json);
    }

    public PlaterStatsData LoadStats()
    {
        if (!File.Exists(path)) return new PlaterStatsData();
        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<PlaterStatsData>(json);
    }
}
