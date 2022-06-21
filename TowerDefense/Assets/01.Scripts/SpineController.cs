using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;
using DG.Tweening;
using System.Text;

public class SpineController : MonoBehaviour
{
    const string normalRun = "Normal_run";
    const string wingRun = "Wing_run";

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

    [HideInInspector] 
    public SkeletonAnimation sa;
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


       if(Define.HasType(this.GetComponent<EnemyBase>().enemyData.MonsterType, Define.MonsterType.Shadow)) // 
       {
           skeleton.A = 0.5f;
       }
    }

    public void Die()
    {
        sa.loop = false;
        sa.AnimationName = dieAnim;
        DOTween.To(() => sa.skeleton.A, value => sa.skeleton.A = value, 0, 0.75f).SetDelay(0.75f);
    }

    public void SetAnim(bool bPlayNormalAnim) 
    {
        string animStr = bPlayNormalAnim ? normalRun : wingRun;
        if (false == sa.AnimationName.Equals(animStr))
        {
            sa.AnimationName = animStr;
        }
    }
}
