using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemySO : ScriptableObject
{
    public string mosterName;               
    public int rewardGold;               // 처치 시 골드 보상
    public int spawnCost;                // 소환 코스트
    public float HP;                     // 체력
    public float shieldAmount;           // 쉴드량
    public float moveSpeed;              // 이동속도
    public MonsterType monsterType;      // 몬스터 특성
    public SpeciesType speciesType;      // 몬스터 종족
    public Sprite sprite;                // 버튼에 사용할 스프라이트
    public Sprite[] typeIcons;           // 특성 아이콘들
    public SpineDataSO spineData;        // 스파인 데이터
    public Enemy basePrefab;             // 베이스 프리팹
}
