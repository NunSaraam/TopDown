using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropManager : MonoBehaviour
{
    public Transform dropSpawnPoint;

    [Header("회차별 무기 드롭 리스트 (1회차 = 인덱스 0)")]
    public List<WeaponSO> loopDropWeapons;

    private void Start()
    {
        Debug.Log("현재 루프 수: " + PlayerStatsManager.Instance.loopCount);
    }

    public void DropWeaponForCurrentLoop()
    {
        int loop = PlayerStatsManager.Instance.loopCount;
        int index = Mathf.Clamp(loop - 1, 0, loopDropWeapons.Count - 1);

        WeaponSO weapon = loopDropWeapons[index];
        if (weapon != null)
        {
            WeaponInventoryManager.Instance.UnlockWeapon(weapon);
            Debug.Log($"회차 {loop} 클리어! 무기 해금: {weapon.weaponName}");

            if (weapon.weaponPrefab != null && dropSpawnPoint != null)
            {
                Instantiate(weapon.weaponPrefab, dropSpawnPoint.position, Quaternion.identity);
                Debug.Log($"무기 프리팹 소환됨: {weapon.weaponName}");
            }
        }
    }
}
