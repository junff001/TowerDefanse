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
}

public class Tower : MonoBehaviour
{
    [SerializeField] private TowerData towerData = new TowerData();              // 인스턴스 타워 정보
    public TowerData TowerData { get => towerData; set => value = towerData; }

    public GameObject attackRangeObj { get; set; } = null;      // 공격 범위 오브젝트
    public Action Act = null; // 버프 디버프 공격 등등

    public Transform coreTrm;

    public void InitTowerData(TowerSO towerSO)
    {
        towerData.Level = towerSO.Level;
        towerData.OffensePower = towerSO.OffensePower;
        towerData.AttackSpeed = towerSO.AttackSpeed;
        towerData.AttackRange = towerSO.AttackRange;
        towerData.PlaceCost = towerSO.PlaceCost;
        towerData.attackTargetCount = towerSO.AttackTargetCount;
        // CoreType에 따라 프리팹 생성
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
