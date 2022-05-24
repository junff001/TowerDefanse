using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class AnimationSkinController : MonoBehaviour
{
    [SpineSkin] public string baseSkinName;
    [SpineSlot] public List<string> slots = new List<string>();
    [SpineAttachment] public string eyeKey; // 고글 어테치먼트의 이름
    public SkeletonAnimation s;

    private void Start()
    {
        Skeleton skeleton = s.skeleton; // 스켈레톤 클래스

        Skin newSkin = new Skin("custom skin"); // 새로운 스킨생성
        var baseSkin = skeleton.Data.FindSkin(baseSkinName); // 기존에 있는 스킨 가져오기
        newSkin.AddSkin(baseSkin); // 기존에 있는 스킨에 정의된 값을 새로운 스킨에 추가

        Debug.Log(skeleton.FindSlot("Head").Attachment.Name);

    }
}
