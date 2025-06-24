using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WeaponSelectUI : MonoBehaviour
{
    [Header("���� ����UI")]
    [SerializeField] GameObject weaponSelectPanel;
    [SerializeField] Transform weaponButtonParent;
    [SerializeField] GameObject weaponButtonPrefab;

    private WeaponSO selectedWeapon;
    private GameObject selectedButton;

    private void Start()
    {
        GenerateWeaponButtons();    
    }

    public void ShowWeaponSelectUI()
    {
        weaponSelectPanel.SetActive(true);
        GenerateWeaponButtons();
    }

    void GenerateWeaponButtons()
    {
        foreach (Transform child in weaponButtonParent)
        {
            Destroy(child.gameObject);
        }

        var weapons = WeaponInventoryManager.Instance.unlockedWeapons;

        foreach (WeaponSO weapon in weapons)
        {
            GameObject btn = Instantiate(weaponButtonPrefab, weaponButtonParent);

            Image iconImage = btn.transform.Find("IconImage").GetComponent<Image>();
            TMP_Text nameText = btn.transform.Find("NameText").GetComponent<TMP_Text>();

            iconImage.sprite = weapon.weaponIcon;
            nameText.text = weapon.weaponName;

            btn.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectWeapon(weapon, btn);
            });

            var outline = btn.transform.Find("Panel").GetComponent<Outline>();
            if (outline != null) outline.enabled = false;
        }
    }

    void SelectWeapon(WeaponSO weapon, GameObject btn)
    {

        selectedWeapon = weapon;
        if (selectedButton != null)
        {
            var prevOutline = selectedButton.transform.Find("Panel").GetComponent<Outline>();
            if (prevOutline != null) prevOutline.enabled = false;
        }

        var currOnutline = btn.transform.Find("Panel").GetComponent<Outline>();
        if (currOnutline != null) currOnutline.enabled = true;

        selectedButton = btn;
        Debug.Log($"���õ� ����: {weapon.weaponName}");
    }

    public void OnStartGame()
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning("���⸦ �������ּ���!");
            return;
        }

        GameSessionData.Instance.SetSelectedWeapon(selectedWeapon);
        SceneManager.LoadScene("Stage");
    }
}
