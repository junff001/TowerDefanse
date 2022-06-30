using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerData : LivingEntityData
{
    public int Level;
    public float HP;
    public float AttackPower;
    public float AttackSpeed;
    public float AttackRange;
    public int PlaceCost;
    public int attackTargetCount;
    public Define.PlaceTileType PlaceTileType;
}
