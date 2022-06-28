using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemySO : ScriptableObject
{
    //고블린 이름 : 단단하고 견고한.. 등등
    [SerializeField] private string myName;
    public string MyName => myName;

    //몬스터 설명
    [TextArea][SerializeField] private string manual;
    public string Manual => manual;

    // 체력
    [SerializeField] private float hp = 0;                          
    public float HP => hp;

    // 쉴드
    [SerializeField] private float shield = 0;
    public float Shield => shield;

    // 공격력
    [SerializeField] private int offensePower = 0;           
    public int OffensePower => offensePower;

    // 이동속도
    [SerializeField] private float moveSpeed = 0f;              
    public float MoveSpeed => moveSpeed;

    // 처치 시 골드 보상
    [SerializeField] private int rewardGold = 0;
    public int RewardGold => rewardGold;

    // 소환 코스트
    [SerializeField] private int spawnCost = 0;
    public int SpawnCost => spawnCost;

    //몬스터의 특성
    [SerializeField] private Define.MonsterType monsterType;
    public Define.MonsterType MonsterType => monsterType;

    //몬스터의 종족
    [SerializeField] private Define.SpeciesType speciesType;
    public Define.SpeciesType SpeciesType => speciesType;

    // 버튼에 사용할 스프라이트
    [SerializeField] private Sprite sprite = null;
    public Sprite Sprite => sprite;

    // 특성 아이콘들
    [SerializeField] private Sprite[] typeIcons= null;
    public Sprite[] TypeIcons => typeIcons;

    // 스파인 데이터
    [SerializeField] private SpineDataSO spineData;
    public SpineDataSO SpineData => spineData;

    // 베이스 프리팹
    [SerializeField] private EnemyBase basePrefab;
    public EnemyBase BasePrefab => basePrefab;
}
