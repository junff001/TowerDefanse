using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpineData", menuName = "ScriptableObjects/SpineData")]
public class SpineDataSO : ScriptableObject
{
    [Header("추가 파츠들")]
    public string[] targetSlots;
    public string[] attachmentKeys; // 고글 어테치먼트의 이름

    [Header("사망 애니메이션 이름")]
    public string dieAnim;

    [Header("달리기 애니메이션 이름")]
    public string runAnim;
}
