using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Boss/NewBoss", fileName = "NewBoss")]
public class BossSO : ScriptableObject
{
    public string bossName;
    public GameObject bossPrefab;

    public int bossHealth;
    public int moveSpeed;
    public int bossDamage;

    public bool canSkill;
    public float skillCooldown;
}
