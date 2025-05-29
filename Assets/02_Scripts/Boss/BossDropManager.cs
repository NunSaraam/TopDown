using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropManager : MonoBehaviour
{
    public BossDropSO dropSequence;
    public int bossClearCount = 0;

    public void DropWeapon()
    {
        if (dropSequence == null || dropSequence.dropSequence.Length == 0)
        {
            return;
        }

        //현재 회차에 맞는 무기 선택
        int index = Mathf.Min(bossClearCount, dropSequence.dropSequence.Length - 1);
        WeaponSO weaponDrop = dropSequence.dropSequence[index];
        
        if (weaponDrop != null)
        {
            Instantiate(weaponDrop.weaponPrefab, transform.position, Quaternion.identity);
            Debug.Log($"Dropped Weapon : {weaponDrop.weaponName}");
        }

        bossClearCount++;
    }

    private void Start()
    {
        bossClearCount = PlayerPrefs.GetInt("BossClearCount", 0);
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("BossClearCount", bossClearCount);
        PlayerPrefs.Save();
    }
}
