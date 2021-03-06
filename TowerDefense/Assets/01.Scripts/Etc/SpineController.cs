using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using DG.Tweening;

public class SpineController : MonoBehaviour
{
    private string dieAnim = string.Empty; // 사망 애니메이션 이름

    //팔 다리 머리같은 기본적으로 장착하는 부분.
    private string[] baseAttachments = 
    {
        "Eye", "HeadParts", "Ear_L", "Ear_R", "Head", "Arm_L", "Arm_R", "Leg_Up_L", "Leg_Up_R", "Leg_Down_L", "Leg_Down_R",  "Body",
        //"Armor_Body", "Shadow_Head", "Shadow_L", "Shadow_R", "Armor_L", "Armor_R", "Guardian","Wing_L","Wing_R"
    };

    [HideInInspector] 
    public SkeletonAnimation sa;

    public Skeleton skeleton;

    public void Init(SpineDataSO spineData)
    {
        sa = GetComponent<SkeletonAnimation>();
        skeleton = sa.skeleton; // 스켈레톤 클래스;

        // 파츠들 전부 해제. 이 친구는 포문 못 돌림.
        foreach (Slot slot in skeleton.Slots)
        {
            slot.Attachment = null;
        }

        // 팔, 다리같은 기본적인 파츠들은 다시 장착하고
        for (int i = 0; i < baseAttachments.Length; i++)
        {
            skeleton.SetAttachment(baseAttachments[i], baseAttachments[i]);
        }

        SetParts(spineData.targetSlots);
        dieAnim = spineData.dieAnim;
        sa.AnimationName = spineData.runAnim;
    }

    public void SetParts(string[] targetSlots)
    {
        if (targetSlots?.Length <= 0) return;

        try
        {
            for (int i = 0; i < targetSlots.Length; i++)
            {
                skeleton.SetAttachment(targetSlots[i], targetSlots[i]);
            }
        }
        catch
        {
            Debug.Log("스파인 파츠 세팅 오류"); 
        }
    }

    public void Die()
    {
        sa.loop = false;
        sa.AnimationName = dieAnim;
        DOTween.To(() => sa.skeleton.A, value => sa.skeleton.A = value, 0, 0.75f).SetDelay(0.75f);
    }
}
