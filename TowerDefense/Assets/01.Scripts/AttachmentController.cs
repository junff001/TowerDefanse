using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class AttachmentController : MonoBehaviour
{
    private string[] baseAttachments =
    {
        "Eye", "HeadParts", "Ear_L", "Ear_R", "Head", "Arm_L", "Arm_R", "Leg_Up_L", "Leg_Up_R", "Leg_Down_L", "Leg_Down_R",  "Body",
        //"Armor_Body", "Shadow_Head", "Shadow_L", "Shadow_R", "Armor_L", "Armor_R", "Guardian","Wing_L","Wing_R"
    };  

    [SpineSlot] public string[] targetSlots;
    [SpineAttachment] public string[] attachmentKeys; // 고글 어테치먼트의 이름

    private void Start()
    {
        //if (attachmentKeys.Length != targetSlots.Length)
        //{
        //    Debug.LogError("두 배열의 길이는 같고, 인덱스 1번에 1번에 넣을 파츠를 골라서 넣어줘야 함!!");
        //    return;
        //}

        SkeletonAnimation sa = GetComponent<SkeletonAnimation>();
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

    }
}
