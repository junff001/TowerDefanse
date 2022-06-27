using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemySO : ScriptableObject
{
    //고블린 이름 : 단단하고 견고한.. 등등
    [SerializeField] private string myName;
    public string MyName { get => myName; set => value = myName; }

    // 체력
    [SerializeField] private float hp = 0;                          
    public float HP { get => hp; set => value = hp; }

    // 쉴드
    [SerializeField] private float shield = 0;
    public float Shield { get => shield; set => value = shield; }

    // 공격력
    [SerializeField] private int offensePower = 0;           
    public int OffensePower { get => offensePower; set => value = offensePower; }

    // 이동속도
    [SerializeField] private float moveSpeed = 0f;              
    public float MoveSpeed { get => moveSpeed; set => value = moveSpeed; }

    // 처치 시 골드 보상
    [SerializeField] private int rewardGold = 0;
    public int RewardGold { get => rewardGold; set => value = rewardGold; }

    // 소환 코스트
    [SerializeField] private int spawnCost = 0;
    public int SpawnCost { get => spawnCost; set => value = spawnCost; }

    //몬스터의 특성
    [SerializeField] private Define.MonsterType monsterType;
    public Define.MonsterType MonsterType { get => monsterType; set => value = monsterType; }

    //몬스터의 종족
    [SerializeField] private Define.SpeciesType speciesType;
    public Define.SpeciesType SpeciesType { get => speciesType; set => value = speciesType; }

    // 버튼에 사용할 스프라이트
    [SerializeField] private Sprite sprite = null;
    public Sprite Sprite { get => sprite; set => value = sprite; }

    // 특성 아이콘들
    [SerializeField] private Sprite[] typeIcons= null;
    public Sprite[] TypeIcons { get => typeIcons; set => value = typeIcons; }

    // 충전 시간
    [SerializeField] private int chargeTime = 0;
    public int ChargeTime { get => chargeTime; set => value = chargeTime; }

    // 스파인 데이터
    [SerializeField] private SpineDataSO spineData;
    public SpineDataSO SpineData { get => spineData; set => value = spineData; }
}
