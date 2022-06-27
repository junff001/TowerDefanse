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
    public int Level { get => level; }

    // 공격력
    [SerializeField] private int attackPower  = 0;           
    public int AttackPower { get => attackPower; }

    // 공격속도
    [SerializeField] private float attackSpeed = 0f;              
    public float AttackSpeed { get => attackSpeed; }

    // 공격범위
    [SerializeField] private float attackRange = 0f;              
    public float AttackRange { get => attackRange; }

    // 설치비용
    [SerializeField] private int placeCost = 100;
    public int PlaceCost { get => placeCost; }

    [SerializeField] private int attackTargetCount = 1;
    public int AttackTargetCount { get => attackTargetCount; }

    public Sprite towerSprite;

    public Define.PlaceTileType placeTileType = Define.PlaceTileType.Place;
    public eCoreName coreType = eCoreName.Bow; // 위 머리 판단기준
    public Define.PropertyType propertyType = Define.PropertyType.NONE;

    public bool hasTower = true;
}
