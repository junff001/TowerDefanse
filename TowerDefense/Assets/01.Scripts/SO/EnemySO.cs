using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemySO : ScriptableObject
{
    public string MosterName;               
    public string Manual;                // 몬스터 설명
    public int OffensePower;             // 공격력
    public int RewardGold;               // 처치 시 골드 보상
    public int SpawnCost;                // 소환 코스트
    public float HP;                     // 체력
    public float ShieldAmount;           // 쉴드량
    public float MoveSpeed;              // 이동속도
    public MonsterType MonsterType;      // 몬스터 특성
    public SpeciesType SpeciesType;      // 몬스터 종족
    public AttackType AttackType;        // 공격 방식
    public Sprite Sprite;                // 버튼에 사용할 스프라이트
    public Sprite[] TypeIcons;           // 특성 아이콘들
    public SpineDataSO SpineData;        // 스파인 데이터
    public EnemyBase BasePrefab;         // 베이스 프리팹
}
