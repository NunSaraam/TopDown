using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeaponSystem : MonoBehaviour
{
    [Header("무기 생성 위치")]
    public Transform weaponHolder;

    [Header("UI 연결")]
    public Image reloadingUI;
    public TextMeshProUGUI ammoUI;

    private GameObject equippedWeapon;

    private void Start()
    {
        WeaponSO selectedWeapon = GameSessionData.Instance.GetSelectedWepon();

        if (selectedWeapon != null)
        {
            EquipWeapon(selectedWeapon);
        }
    }

    public void EquipWeapon(WeaponSO weaponSO)
    {
        if (equippedWeapon != null)
        {
            Destroy(equippedWeapon);
        }

        equippedWeapon = Instantiate(weaponSO.weaponPrefab, weaponHolder);
        equippedWeapon.transform.localPosition = Vector3.zero;

        var weapondata = equippedWeapon.GetComponent <WeaponData>();

        if (weapondata != null)
        {
            weapondata.SetWeaponSO(weaponSO);
            weapondata.reloadingUI = reloadingUI;
            weapondata.ammoUI = ammoUI;
        }

        Aiming aiming = GetComponent<Aiming>();
        if (aiming != null)
        {
            aiming.SetWeaponData(weapondata);

            Transform firepoint = equippedWeapon.transform.Find("FirePoint");
            if (firepoint != null)
            {
                aiming.SetFirePoint(firepoint);
                Debug.Log($" FirePoint 연결됨: {firepoint.name}");
            }
            else
            {
                Debug.LogError(" FirePoint를 찾지 못했습니다.");
            }
        }
    }
}
