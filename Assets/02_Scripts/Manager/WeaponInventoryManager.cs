using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class WeaponInventoryManager : MonoBehaviour
{
    public static WeaponInventoryManager Instace {  get; private set; }

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
        if (Instace != null && Instace != this)
        {
            Destroy(gameObject);
            return;
        }
        Instace = this;
        DontDestroyOnLoad(gameObject);
        LoadInventory();
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

    public void SaveInventory()
    {
        WeaponinventorySaveData saveData = new WeaponinventorySaveData();

        foreach (var weapon in allWeapons)
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
