using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GoldManager : Singleton<GoldManager>
{
    public int Gold
    {
        get
        {
            return GameManager.Instance.Gold;
        }
        set
        {
            GameManager.Instance.Gold = value;
        }
    }

    public void GoldPlus(int plus)
    {
        Gold += plus;
        UIManager.Instance.UpdateGoldText();
    }

    public bool GoldMinus(int cost)
    {
        if (Gold >= cost)
        {
            Gold -= cost;
            Debug.Log($"골드 {cost} 차감");
            UIManager.Instance.UpdateGoldText();
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다");
            UIManager.Instance.moneyText.DOComplete();
            UIManager.Instance.moneyText.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);
            return false;
        }
    }
}
