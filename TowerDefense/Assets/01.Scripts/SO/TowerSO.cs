using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum CoreType
{
    Base
}

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData")]
public class TowerSO : ScriptableObject
{
    // 레벨
    [SerializeField] private int level = 0;
    public int Level { get => level; set => value = level; }

    // 공격력
    [SerializeField] private float offensePower  = 0f;           
    public float OffensePower { get => offensePower; set => value = offensePower; }

    // 공격속도
    [SerializeField] private float attackSpeed = 0f;              
    public float AttackSpeed { get => attackSpeed; set => value = attackSpeed; }

    // 공격범위
    [SerializeField] private float attackRange = 0f;              
    public float AttackRange { get => attackRange; set => value = attackRange; }

    // 설치비용
    [SerializeField] private int placeCost = 100;
    public int PlaceCost { get => placeCost; set => value = placeCost; }

    public CoreType coreType = CoreType.Base;
}
