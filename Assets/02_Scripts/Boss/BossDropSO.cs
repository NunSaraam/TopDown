using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Boss/BossDrop", fileName = "NewBossDrop")]
public class BossDropSO : ScriptableObject
{
    public string bossName;
    public WeaponSO[] dropSequence;
}
