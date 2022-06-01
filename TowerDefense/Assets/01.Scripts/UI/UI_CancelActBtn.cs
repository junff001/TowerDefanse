using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_CancelActBtn : MonoBehaviour
{
    public ActData actData = null;
    public Button cancelActBtn;
    public int actStackCount = 0;
    public int idx = 0;

    public Image monsterImg;
    public Text countText;

    public void Stack() // 쌓기
    {
        actStackCount++;
        countText.text = actStackCount.ToString();
    }

    public void Cancel()
    {
        Managers.Invade.OnCancelAct(actData);
        actStackCount--;
        countText.text = actStackCount.ToString();
        DestroyCheck();
    }

    public void Init(ActData actData)
    {
        this.actData = actData;
        this.monsterImg.sprite = Managers.Game.GetActBtnSprite(actData.monsterType);
        this.name = actData.monsterType.ToString();
    }
        
    public void DestroyCheck() // todo 풀매니저
    {
        if (actStackCount == 0)
        {
            InvadeManager im = Managers.Invade;
            im.waitingActs.Remove(this);

            if (im.waitingActs.Count > 0)
            {
                im.addedAct = im.waitingActs[im.waitingActs.Count - 1].actData;
                im.addedBtn = im.waitingActs[im.waitingActs.Count - 1];
                Managers.Invade.OnBtnRemoved(idx);
            }
            else
            {
                im.addedBtn = null;
                im.addedAct = null;
            }

            Destroy(this.gameObject);
        }
    }
}
