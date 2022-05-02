using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData", order = int.MaxValue)]
public class TowerSO : ScriptableObject
{
    // 레벨
    [SerializeField] private int level = 0;
    public int Level { get => level; set => value = level; }
    // 체력
    [SerializeField] private int hp = 0;                          
    public int HP { get => hp; set => value = hp; }

    // 공격력
    [SerializeField] private float offensePower  = 0f;           
    public float OffensePower { get => offensePower; set => value = offensePower; }

    // 공격속도
    [SerializeField] private float attackSpeed = 0f;              
    public float AttackSpeed { get => attackSpeed; set => value = attackSpeed; }

    // 공격범위
    [SerializeField] private float attackRange = 0f;              
    public float AttackRange { get => attackRange; set => value = attackRange; }
}
