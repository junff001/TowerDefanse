using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Test : MonoBehaviour
{
    [SpineSkin] public string baseSkinName;
    [SpineSlot] public string eyeSlot;
    [SpineAttachment] public string eyeKey; // ��� ����ġ��Ʈ�� �̸�
    public SkeletonAnimation s;

    private void Start()
    {
        Skeleton skeleton = s.Skeleton; // ���̷��� Ŭ����

        Skin newSkin = new Skin("custom skin"); // ���ο� ��Ų����
        var baseSkin = skeleton.Data.FindSkin(baseSkinName); // ������ �ִ� ��Ų ��������
        //newSkin.SetAttachment(baseSkin); // ������ �ִ� ��Ų�� ���ǵ� ���� ���ο� ��Ų�� �߰�
    }
}
