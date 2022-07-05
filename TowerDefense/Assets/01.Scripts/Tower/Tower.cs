using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : LivingEntity
{
    [HideInInspector] public TowerData TowerData;
    [SerializeField] private GameObject healthbar;

    private HealthSystem healthSystem;
    private string initSortingLayerName;
    private int initOrderInLayer;

    public Vector3Int[] myCheckedPos { get; set; } // 나의 그리드값을 가진다.
    public Transform CoreTrm;   // position 접근 변수
   
    void Awake()
    {
        livingEntityData = new TowerData();
        TowerData = livingEntityData as TowerData;

        healthSystem = GetComponent<HealthSystem>();
    }

    public void InitTowerData(TowerSO towerSO, int addPctTowerAtkPower)
    {
        TowerData.Level               = towerSO.Level;
        TowerData.HP                  = towerSO.HP;
        TowerData.ShieldAmount        = towerSO.ShieldAmount;
        TowerData.AttackPower         = towerSO.AttackPower;
        TowerData.AttackSpeed         = towerSO.AttackSpeed;
        TowerData.AttackRange         = towerSO.AttackRange;
        TowerData.PlaceCost           = towerSO.PlaceCost;
        TowerData.attackTargetCount   = towerSO.AttackTargetCount;
        TowerData.PlaceTileType       = towerSO.placeTileType;

        TowerData.AttackPower += TowerData.AttackPower * addPctTowerAtkPower;
    }

    public void SetHealthBar()
    {
        healthSystem.livingEntity = this;
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)TowerData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)TowerData.ShieldAmount, true);
        healthbar.SetActive(true);
    }

    private void Update()
    {
        //Debug.Log("현재 타워 체력 : " + TowerData.HP);
    }

    private void OnMouseDown()
    {
        if(Managers.Invade.isSelectingTower)
        {
            Managers.Invade.SpawnEnemy(Managers.Invade.GetEnemySO(), this.transform);
            return;
        }

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
