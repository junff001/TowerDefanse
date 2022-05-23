using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_GuideText : MonoBehaviour
{
    public Text guideText;

    public void ShowGuide(Vector3 movePos, bool costEnough = true, bool canPlace = true)// 설치비용 충분, 위치 적절
    {

        if(costEnough == false)
        {
            guideText.text = "타워 설치 비용이 부족합니다.";
        }
        else if(canPlace == false)
        {
            guideText.text = "설치 불가능한 위치입니다.";
        }
        else
        {
            guideText.text = "설치 완료!";
        }

        transform.position = movePos;
        guideText.DOKill();
        guideText.DOFade(1, 1.5f).OnComplete(() => guideText.DOFade(0, 0.2f));
    }
}
