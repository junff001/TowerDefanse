using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class GoldManager : MonoBehaviour
{
    public Text moneyText;

    public int Gold { get; set; } = 250;

    private void Start()
    {
        UpdateGoldText();
    }

    public void UpdateGoldText()
    {
        moneyText.text = Gold.ToString();
    }

    public void GoldSet(int value)
    {
        Gold = value;
        UpdateGoldText();
    }

    public void GoldPlus(int plus)
    {
        Gold += plus;
        UpdateGoldText();
    }

    public void GoldPlus(float plus)
    {
        int parsed_value = (int)plus;

        Gold += parsed_value;
        UpdateGoldText();
    }

    public bool GoldMinus(int cost)
    {
        if (Gold >= cost)
        {
            Gold -= cost;
            Debug.Log($"골드 {cost} 소모");
            UpdateGoldText();
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다");
            moneyText.DOComplete();
            moneyText.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);
            return false;
        }
    }

    public bool GoldMinus(float cost)
    {
        int parsed_value = (int)cost;

        if (Gold >= parsed_value)
        {
            Gold -= parsed_value;
            Debug.Log($"골드 {parsed_value} 소모");
            UpdateGoldText();
            return true;
        }
        else
        {
            Debug.Log("골드가 부족합니다");
            moneyText.DOComplete();
            moneyText.DOColor(Color.red, 0.15f).SetLoops(2, LoopType.Yoyo);
            return false;
        }
    }
}
