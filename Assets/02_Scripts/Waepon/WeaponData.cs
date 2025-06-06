using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponData : MonoBehaviour
{
    [SerializeField] WeaponSO data;

    private int currentAmmo;
    private bool isReloading = false;
    private float timer = 0f;
    private float reloadTime = 2f;
    
    public Image reloadingUI;
    public TextMeshProUGUI ammoUI;

    private void Start()
    {
        currentAmmo = data.maxAmmo;
        ammoUI.text = $"{currentAmmo}/{data.maxAmmo}";
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
        
        ammoUI.text = $"{currentAmmo}/{data.maxAmmo}";

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
            currentAmmo = data.maxAmmo;

            ammoUI.text = $"{currentAmmo}/{data.maxAmmo}";

            reloadingUI.gameObject.SetActive(false);
        }
    }

    public int GetMaxAmmo()
    {
        return data.maxAmmo;
    }
}
