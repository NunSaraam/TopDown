using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;


public class PlaterStatsData
{
    public int deathCount;
    public int loopCount;
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

    public void SaveStats(int deathCount, int loopCount)
    {
        PlaterStatsData data = new PlaterStatsData();
        data.deathCount = deathCount;
        data.loopCount = loopCount;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/stats.json", json);
    }

    public PlaterStatsData LoadStats()
    {
        string path = Application.persistentDataPath + "/stats.json";
        if (!File.Exists(path))
        {
            return new PlaterStatsData();
        }

        string json = File.ReadAllText(path);
        return JsonUtility.FromJson<PlaterStatsData>(json);
    }
}
