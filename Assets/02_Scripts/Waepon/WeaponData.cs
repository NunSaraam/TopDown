using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponData : MonoBehaviour
{
    [SerializeField] WeaponSO weaponSO;

    private int currentAmmo;
    private bool isReloading = false;
    private float timer = 0f;
    private float reloadTime = 0.5f;
    
    public Image reloadingUI;
    public TextMeshProUGUI ammoUI;

    private void Start()
    {
        currentAmmo = weaponSO.maxAmmo;
        ammoUI.text = $"{currentAmmo}/{weaponSO.maxAmmo}";
        reloadingUI.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isReloading)
        {
            Reload();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            TryReload();
        }
    }

    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }

    public void ConsumeAmmo()
    {
        currentAmmo = Mathf.Max(0, currentAmmo - 1);
        Debug.Log("ÇöÀç Åº¾à : " + currentAmmo);
        
        ammoUI.text = $"{currentAmmo}/{weaponSO.maxAmmo}";

        TryReload();
    }

    public void TryReload()
    {
        if (Input.GetKeyDown(KeyCode.R) || currentAmmo <= 0)
        {
            if (!isReloading)
            {
                isReloading = true;
                timer = 0f;
                reloadingUI.gameObject.SetActive(true);
            }
        }

    }

    public void Reload()
    {
        timer += Time.deltaTime;

        reloadingUI.fillAmount = timer / reloadTime;

        if (timer >= reloadTime)
        {
            timer = 0f;
            isReloading = false;
            currentAmmo = weaponSO.maxAmmo;

            ammoUI.text = $"{currentAmmo}/{weaponSO.maxAmmo}";

            reloadingUI.gameObject.SetActive(false);
        }
    }
    public bool IsReloading()
    {
        return isReloading;
    }

    public int GetMaxAmmo()
    {
        return weaponSO.maxAmmo;
    }

    public float GetShootRate()
    {
        return weaponSO.shootRate;
    }

    public bool IsAuto()
    {
        return weaponSO.isAuto;
    }

    public void SetWeaponSO(WeaponSO so)
    {
        weaponSO = so;
    }
}
