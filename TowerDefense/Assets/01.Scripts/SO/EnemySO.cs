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

    // 은신 여부
    [SerializeField] private bool isHide = false;
    public bool IsHide { get => isHide; set => value = isHide; }
}
