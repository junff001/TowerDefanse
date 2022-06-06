using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class SpineController : MonoBehaviour
{
    private string[] baseAttachments =
    {
        "Eye", "HeadParts", "Ear_L", "Ear_R", "Head", "Arm_L", "Arm_R", "Leg_Up_L", "Leg_Up_R", "Leg_Down_L", "Leg_Down_R",  "Body",
        //"Armor_Body", "Shadow_Head", "Shadow_L", "Shadow_R", "Armor_L", "Armor_R", "Guardian","Wing_L","Wing_R"
    };

    [Header("추가 파츠들")]
    [SpineSlot] public string[] targetSlots;
    [SpineAttachment] public string[] attachmentKeys; // 고글 어테치먼트의 이름

    [Header("애니메이션 이름")]
    [SpineAnimation] public string dieAnim;

    SkeletonAnimation sa;
    private void Start()
    {
        sa = GetComponent<SkeletonAnimation>();
        Skeleton skeleton = sa.skeleton; // 스켈레톤 클래스

        foreach(Slot slot in skeleton.Slots) slot.Attachment = null;

       for (int i = 0; i < baseAttachments.Length; i++)
       {
           skeleton.SetAttachment(baseAttachments[i], baseAttachments[i]);
       }

       for (int i = 0; i< targetSlots.Length; i++)
       {
           skeleton.SetAttachment(targetSlots[i], attachmentKeys[i]);
       }


       if(this.GetComponent<EnemyBase>().enemyData.IsHide == true)
        {
            skeleton.A = 0.5f;
        }
    }

    public void Die()
    {
        sa.loop = false;
        sa.AnimationName = dieAnim;
    }
}
