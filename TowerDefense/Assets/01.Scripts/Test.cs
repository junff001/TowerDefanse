using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class Test : MonoBehaviour
{
    public SkeletonAnimation s;
    public Material mat;
    public string[] changeAttachments;

    public void ChangeTargetAttachmentsColor()
    {
        for (int i = 0; i < changeAttachments.Length; i++)
        {
            SlotData slot = s.skeletonDataAsset.GetSkeletonData(false).FindSlot(changeAttachments[i]);
            slot.R = 10;
            slot.G = 255;
            slot.B = 255;
        }
        s.Initialize(true);
    }
}
