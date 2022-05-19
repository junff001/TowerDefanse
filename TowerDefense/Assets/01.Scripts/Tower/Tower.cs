using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct TowerData
{
    public int Level;
    public int OffensePower;
    public float AttackSpeed;
    public float AttackRange;
    public int PlaceCost;
    public int attackTargetCount;
}

public class Tower : MonoBehaviour
{
    private TowerData towerData = new TowerData();              // 인스턴스 타워 정보
    public TowerData TowerData { get => towerData; set => value = towerData; }

    public GameObject attackRangeObj { get; set; } = null;      // 공격 범위 오브젝트
    
    public SpriteRenderer spriteRenderer { get; set; } = null;

    public Action Act = null; // 버프 디버프 공격 등등

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void InitTowerData(TowerSO towerSO)
    {
        towerData.Level = towerSO.Level;
        towerData.OffensePower = towerSO.OffensePower;
        towerData.AttackSpeed = towerSO.AttackSpeed;
        towerData.AttackRange = towerSO.AttackRange;
        towerData.PlaceCost = towerSO.PlaceCost;

        //CoreType에 따라 프리팹 생성
    }

    

    // 공격 범위를 표시하는 함수
    public void AttackRangeActive()
    {

    }

    // 공격 범위 기즈모 표시
#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, towerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif
}
