using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSessionData : MonoBehaviour
{
    public static GameSessionData Instance {  get; private set; }

    private WeaponSO selectedWeapon;

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

    public void SetSelectedWeapon(WeaponSO weapon)
    {
        selectedWeapon = weapon;
    }

    public WeaponSO GetSelectedWepon()
    {
        return selectedWeapon;
    }
}
