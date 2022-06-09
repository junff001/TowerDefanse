using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TowerData
{
    public int Level;
    public int OffensePower;
    public float AttackSpeed;
    public float AttackRange;
    public int PlaceCost;
    public int attackTargetCount;
    public Define.PropertyType property;
}

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData towerData = new TowerData();// 인스턴스 타워 정보
    public TowerData TowerData { get => towerData; set => value = towerData; }

    public GameObject attackRangeObj { get; set; } = null;      // 공격 범위 오브젝트
    public Transform coreTrm;
    private List<BuffBase> buffList = new List<BuffBase>();

    [SerializeField] private GameObject lightAura;
    [SerializeField] private GameObject lightningAura;
    [SerializeField] private GameObject fireAura;
    [SerializeField] private GameObject darknessAura;
    [SerializeField] private GameObject waterAura;

    public void InitTowerData(TowerSO towerSO)
    {
        towerData.Level = towerSO.Level;
        towerData.OffensePower = towerSO.OffensePower;
        towerData.AttackSpeed = towerSO.AttackSpeed;
        towerData.AttackRange = towerSO.AttackRange;
        towerData.PlaceCost = towerSO.PlaceCost;
        towerData.attackTargetCount = towerSO.AttackTargetCount;
        towerData.property = towerSO.propertyType;


        GameObject makeObj = null;
        switch(towerData.property)
        {
            case Define.PropertyType.DARKNESS: makeObj = darknessAura; break;
            case Define.PropertyType.LIGHT: makeObj = lightAura; break;
            case Define.PropertyType.LIGHTNING: makeObj = lightningAura; break;
            case Define.PropertyType.WATER: makeObj = waterAura; break;
            case Define.PropertyType.FIRE: makeObj = fireAura; break;
        }

        if(makeObj != null)
        {
            Instantiate(makeObj, this.transform);
        }
    }
}
