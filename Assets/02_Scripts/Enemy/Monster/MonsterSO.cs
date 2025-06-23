using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Monster/MonsterData", fileName = "NewMonster")]
public class MonsterSO : ScriptableObject
{
    public string monsterName;
    public Sprite monsterIcon;
    public GameObject monsterPrefab;
    
    public int monsterHealth;
    public float moveSpeed;
    public int attackDamage;
    public float attackRange;
    public float attackCooldown;

    public RuntimeAnimatorController animatorController;
    public GameObject HealthItemPrefab;

    public bool canSkill;
    public float skillCooldown;
}
