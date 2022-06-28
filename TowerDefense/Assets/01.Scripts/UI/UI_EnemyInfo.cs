using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class UI_EnemyInfo : MonoBehaviour
{
    [SerializeField] private Image monsterImg;
    [SerializeField] private TextMeshProUGUI costText; // 공격력
    [SerializeField] private TextMeshProUGUI hpText    ;   // 체력
    [SerializeField] private TextMeshProUGUI speedText ;   // 이동속도
    [SerializeField] private TextMeshProUGUI manualText;

    public void Init(EnemySO so)
    {
        monsterImg.sprite = so.Sprite;

        hpText.SetText(so.HP.ToString());
        speedText.text = so.MoveSpeed.ToString();
        costText.text = so.SpawnCost.ToString();
        manualText.text = so.Manual;

        gameObject.SetActive(false);
    }
}
