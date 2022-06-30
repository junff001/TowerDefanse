using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingEntity
{
    public TowerData towerData = new TowerData();

    public GameObject attackRangeObj { get; set; } = null;      // 공격 범위 오브젝트
    public Transform coreTrm;

    public Vector3Int[] myCheckedPos { get; set; } // 나의 그리드값을 가진다.

    private List<BuffBase> buffList = new List<BuffBase>();

    private GameObject propertyEffectObject;

    private string initSortingLayerName;
    private int initOrderInLayer;

    public void InitTowerData(TowerSO towerSO, int addPctTowerAtkPower)
    {
        towerData.Level = towerSO.Level;
        towerData.HP = towerSO.HP;
        towerData.AttackPower = towerSO.AttackPower;
        towerData.AttackSpeed = towerSO.AttackSpeed;
        towerData.AttackRange = towerSO.AttackRange;
        towerData.PlaceCost = towerSO.PlaceCost;
        towerData.attackTargetCount = towerSO.AttackTargetCount;
        towerData.PlaceTileType = towerSO.placeTileType;

        towerData.AttackPower += towerData.AttackPower * addPctTowerAtkPower;
    }

    private void OnMouseDown()
    {
        if (Managers.Wave.GameMode == Define.GameMode.DEFENSE && Time.timeScale > 0)
        {
            Managers.Game.towerInfoUI.OpenInfo(this);
        }
    }

    public void InitSortOrder(string layerName, int orderInLayer) // 생성때 지정된 오더값, 리셋하면 이값으로 바뀜
    {
        initSortingLayerName = layerName;
        initOrderInLayer = orderInLayer;

        SetSortOrder(layerName, orderInLayer);
    }

    public void ResetSortOrder()
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingLayerName = initSortingLayerName;
            item.sortingOrder = initOrderInLayer;
        }
    }

    public void SetSortOrder(string layerName, int orderInLayer) // 임시적으로 오더값 변경시 사용
    {
        foreach (var item in GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingLayerName = layerName;
            item.sortingOrder = orderInLayer;
        }
    }
}
