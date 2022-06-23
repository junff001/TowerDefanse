using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpineData", menuName = "ScriptableObjects/SpineData")]
public class SpineDataSO : ScriptableObject
{
    [Header("추가 파츠들!")]
    [Tooltip("슬롯의 이름과 사용할 파츠의 이름이 같아요")]
    public string[] targetSlots;

    [Header("사망 애니메이션 이름")]
    public string dieAnim;

    [Header("달리기 애니메이션 이름")]
    public string runAnim;
}
