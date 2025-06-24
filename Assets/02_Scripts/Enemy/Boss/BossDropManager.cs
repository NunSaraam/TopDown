using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDropManager : MonoBehaviour
{
    public Transform dropSpawnPoint;

    [Header("ȸ���� ���� ��� ����Ʈ (1ȸ�� = �ε��� 0)")]
    public List<WeaponSO> loopDropWeapons;

    private void Start()
    {
        Debug.Log("���� ���� ��: " + PlayerStatsManager.Instance.loopCount);
    }

    public void DropWeaponForCurrentLoop()
    {
        int loop = PlayerStatsManager.Instance.loopCount;
        int index = Mathf.Clamp(loop - 1, 0, loopDropWeapons.Count - 1);

        WeaponSO weapon = loopDropWeapons[index];
        if (weapon != null)
        {
            WeaponInventoryManager.Instance.UnlockWeapon(weapon);
            Debug.Log($"ȸ�� {loop} Ŭ����! ���� �ر�: {weapon.weaponName}");

            if (weapon.weaponPrefab != null && dropSpawnPoint != null)
            {
                Instantiate(weapon.weaponPrefab, dropSpawnPoint.position, Quaternion.identity);
                Debug.Log($"���� ������ ��ȯ��: {weapon.weaponName}");
            }
        }
    }
}
