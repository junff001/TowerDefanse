using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class AnimationSkinController : MonoBehaviour
{
    //[SpineSlot] public List<string> slots = new List<string>();
    [SpineSlot] public string[] targetSlots;
    [SpineAttachment(currentSkinOnly =true)] public string[] attachmentKey; // 고글 어테치먼트의 이름
    private SkeletonAnimation sa;

    private void Start()
    {
        if(attachmentKey.Length != targetSlots.Length)
        {
            Debug.LogError("두 배열의 길이는 같고, 인덱스 1번에 1번에 넣을 파츠를 골라서 넣어줘야 함!!");
            return;
        }

        sa = GetComponent<SkeletonAnimation>();

        Skeleton skeleton = sa.skeleton; // 스켈레톤 클래스

        //skeleton.Slots.ForEach((x) => x.Attachment = null);
        for(int i = 0; i< targetSlots.Length; i++)
        {
            //skeleton.SetAttachment(targetSlots[i], attachmentKey[i]);
            skeleton.FindSlot(targetSlots[i]).A = 0;
        }

    }
}
