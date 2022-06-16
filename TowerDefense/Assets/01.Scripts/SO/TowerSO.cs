using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eCoreName
{
    Canon,
    Bow,
    Catapult,
    Spike,
    WoodenBarrel,
    Jewel,
    GravityField,
    Sensor,
    AtkBuff,
    AtkSpeedBuff,
    AtkRangeBuff,
    SlowDeBuff,

}

[CreateAssetMenu(fileName = "TowerData", menuName = "ScriptableObjects/TowerData")]
public class TowerSO : ScriptableObject
{
    // 레벨
    [SerializeField] private int level = 0;
    public int Level { get => level; set => value = level; }

    // 공격력
    [SerializeField] private int offensePower  = 0;           
    public int OffensePower { get => offensePower; set => value = offensePower; }

    // 공격속도
    [SerializeField] private float attackSpeed = 0f;              
    public float AttackSpeed { get => attackSpeed; set => value = attackSpeed; }

    // 공격범위
    [SerializeField] private float attackRange = 0f;              
    public float AttackRange { get => attackRange; set => value = attackRange; }

    // 설치비용
    [SerializeField] private int placeCost = 100;
    public int PlaceCost { get => placeCost; set => value = placeCost; }

    [SerializeField] private int attackTargetCount = 1;
    public int AttackTargetCount { get => attackTargetCount; set => value = attackTargetCount; }

    public Sprite towerSprite;

    public Define.PlaceTileType placeTileType = Define.PlaceTileType.Place;
    public eCoreName coreType = eCoreName.Bow; // 위 머리 판단기준
    public Define.PropertyType propertyType = Define.PropertyType.NONE;

    public bool hasTower = true;
}
