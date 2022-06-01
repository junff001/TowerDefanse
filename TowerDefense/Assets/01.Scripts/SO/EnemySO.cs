using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/EnemyData")]
public class EnemySO : ScriptableObject
{
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

    // 은신 여부
    [SerializeField] private bool isHide = false;
    public bool IsHide { get => isHide; set => value = isHide; }

    // 수호자 여부
    [SerializeField] private bool isGuardian = false;
    public bool IsGuardian { get => isGuardian; set => value = isGuardian; }

    // 비행 여부
    [SerializeField] private bool isFlying = false;
    public bool IsFlying { get => isFlying; set => value = isFlying; }

    // 속성 저항
    [SerializeField] private Define.PropertyType propertyResistance = Define.PropertyType.NONE;
    public Define.PropertyType PropertyResistance { get => propertyResistance; set => value = propertyResistance; }

    // 디버프 면역 여부
    [SerializeField] private bool isDebuffIimmune = false;
    public bool IsDebuffIimmune { get => isDebuffIimmune; set => value = isDebuffIimmune; }

    [SerializeField] private Define.MonsterType monsterType;
    public Define.MonsterType MonsterType { get => monsterType; set => value = monsterType; }
}
