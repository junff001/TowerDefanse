using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "ScriptableObjects/BuffData")]
public class BuffSO : ScriptableObject
{
    #region 버프
    [Header("버프")]
    // 추가 공격력
    [SerializeField] private int addOffense = 0;
    public int AddOffense { get => addOffense; set => value = addOffense; }

    // 추가 공속
    [SerializeField] private float addAttackSpeed = 0f;
    public float AddAttackSpeed { get => addAttackSpeed; set => value = addAttackSpeed; }

    // 추가 사거리
    [SerializeField] private float addRange = 0f;
    public float AddRange { get => addRange; set => value = addRange; }
    #endregion

    #region 디버프
    [Header("디버프")]
    // 슬로우 퍼센트
    [SerializeField] private float slow_Percentage = 0f;
    public float Slow_Percentage { get => slow_Percentage; set => value = slow_Percentage; }

    // 추가 도트 데미지
    [SerializeField] private float addDotDamage = 0f;
    public float AddDotDamage { get => addDotDamage; set => value = addDotDamage; }
    #endregion

    // 지속 시간
    [SerializeField] private float duration = 0f;
    public float Duration { get => duration; set => value = duration; }    
}
