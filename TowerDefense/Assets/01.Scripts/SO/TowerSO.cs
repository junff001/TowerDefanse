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
    public int Level;
    public int PlaceCost;
    public int AttackTargetCount;
    public float HP;
    public float ShieldAmount;
    public float AttackPower;
    public float AttackSpeed;
    public float AttackRange;
    public Define.PlaceTileType placeTileType = Define.PlaceTileType.Place;
    public eCoreName coreType = eCoreName.Bow; // 위 머리 판단기준

    [HideInInspector] public bool hasTower = true;
}
