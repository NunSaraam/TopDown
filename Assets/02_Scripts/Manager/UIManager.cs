using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("플레이어 체력UI")]
    public Sprite emptyHealth;
    public Sprite fullHealth;
    public Image[] healths;

    [Header("무기 선택UI")]
    [SerializeField] GameObject weaponSelectPanel;
    [SerializeField] Transform weaponButtonParent;
    [SerializeField] GameObject weaponButtonPrefab;

    private WeaponSO selectedWeapon;
    private GameObject selectedButton = null;


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

    private void Start()
    {
        
    }

    public void UpdataHealthUI(int currentHealth, int maxHealth)
    {
        for (int i = 0; i < healths.Length; i++)
        {
            if (i < currentHealth)
            {
                healths[i].sprite = fullHealth;
            }
            else
            {
                healths[i].sprite = emptyHealth;
            }

            healths[i].enabled = (i < maxHealth);
        }
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

        var weapons = WeaponInventoryManager.Instace.unlockedWeapons;

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
        Debug.Log($"선택된 무기: {weapon.weaponName}");
    }

    public void OnStartGame()
    {
        if (selectedWeapon == null)
        {
            Debug.LogWarning("무기를 선택해주세요!");
            return;
        }

        GameSessionData.Instance.SetSelectedWeapon(selectedWeapon);
        SceneManager.LoadScene("Stage");
    }
}
