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
    public int Level => level;

    // 체력
    [SerializeField] private float hp = 0;
    public float HP => hp;

    // 공격력
    [SerializeField] private float attackPower  = 0;           
    public float AttackPower => attackPower; 

    // 공격속도
    [SerializeField] private float attackSpeed = 0f;              
    public float AttackSpeed => attackSpeed; 

    // 공격범위
    [SerializeField] private float attackRange = 0f;              
    public float AttackRange => attackRange; 

    // 설치비용
    [SerializeField] private int placeCost = 100;
    public int PlaceCost => placeCost; 

    [SerializeField] private int attackTargetCount = 1;
    public int AttackTargetCount => attackTargetCount;

    public Define.PlaceTileType placeTileType = Define.PlaceTileType.Place;
    public eCoreName coreType = eCoreName.Bow; // 위 머리 판단기준

    [HideInInspector] public bool hasTower = true;
}
