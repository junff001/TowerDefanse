using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_EnemyInfo : MonoBehaviour
{
    [SerializeField] private Image monsterImg;
    [SerializeField] private Text costText; // 공격력
    [SerializeField] private Text hpText    ;   // 체력
    [SerializeField] private Text speedText ;   // 이동속도
    [SerializeField] private Text manualText;

    public void Init(EnemySO so)
    {
        monsterImg.sprite = so.Sprite;

        hpText.text = so.HP.ToString();
        speedText.text = so.MoveSpeed.ToString();
        costText.text = so.SpawnCost.ToString();
        manualText.text = so.Manual;
    }

}
