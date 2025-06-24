using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{
    public static WeaponInventoryManager Instance {  get; private set; }

    [Header("전체 무기 목록")]
    public List<WeaponSO> allWeapons;

    public List<WeaponSO> unlockedWeapons = new List<WeaponSO>();

    private string savePath => Application.persistentDataPath + "/weapon_inventory.json";

    [System.Serializable]
    private class WeaponinventorySaveData
    {
        public List<string> unlockedWeaponIDs = new List<string>();
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInventory();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            WeaponInventoryManager.Instance.ResetInventory();
            PlayerStatsManager.Instance.ResetStats();
        }
    }

    public void UnlockWeapon(WeaponSO weapon)
    {
        if (!unlockedWeapons.Contains(weapon))
        {
            unlockedWeapons.Add(weapon);
            SaveInventory();
            Debug.Log($"무기 해금됨: {weapon.weaponName}");
        }
    }

    public bool IsUnlocked(WeaponSO weapon)
    {
        return unlockedWeapons.Contains(weapon);
    }

    public void ResetInventory()
    {
        unlockedWeapons.Clear();

        // JSON 파일도 삭제
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("[Inventory] 무기 인벤토리 초기화됨. JSON 삭제됨.");
        }
        else
        {
            Debug.Log("[Inventory] JSON 파일이 존재하지 않습니다.");
        }
    }


    public void SaveInventory()
    {
        WeaponinventorySaveData saveData = new WeaponinventorySaveData();

        foreach (var weapon in unlockedWeapons)
        {
            saveData.unlockedWeaponIDs.Add(weapon.weaponID);
        }

        string json = JsonUtility.ToJson(saveData, true);
        File.WriteAllText(savePath, json);
    }

    public void LoadInventory()
    {
        if (!File.Exists(savePath)) return;

        string json = File.ReadAllText(savePath);
        WeaponinventorySaveData saveData = JsonUtility.FromJson<WeaponinventorySaveData>(json);

        unlockedWeapons.Clear();

        foreach (string id in saveData.unlockedWeaponIDs)
        {
            WeaponSO found = allWeapons.Find(w => w.weaponID == id);
            if (found != null)
            {
                unlockedWeapons.Add(found);
            }
        }
    }
}
