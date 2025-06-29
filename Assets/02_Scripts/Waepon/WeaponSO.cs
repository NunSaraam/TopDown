using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Weapon/WaeponData", fileName = "NewWeapon")]
public class WeaponSO : ScriptableObject
{
    public string weaponID;
    public string weaponName;
    public Sprite weaponIcon;
    public GameObject weaponPrefab;
    public bool isAuto;
    public int maxAmmo;
    public int damage;
    public float shootRate;
}
