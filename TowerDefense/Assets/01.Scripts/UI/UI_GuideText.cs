using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_GuideText : MonoBehaviour
{
    public void ShowGuide(Vector3 movePos, bool costEnough = true, bool canPlace = true)// 설치비용 충분, 위치 적절
    {

        if(costEnough == false)
        {
            UIManager.SummonText(new Vector2(movePos.x, movePos.y), "타워 설치 비용이 부족합니다.", 30);
        }
        else if(canPlace == false)
        {
            UIManager.SummonText(new Vector2(movePos.x, movePos.y), "설치 불가능한 위치입니다.", 30);
        }
        else
        {
            UIManager.SummonText(new Vector2(movePos.x, movePos.y), "설치 완료!", 30);
        }
    }
}
