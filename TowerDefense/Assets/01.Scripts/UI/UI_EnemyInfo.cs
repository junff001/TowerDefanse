using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Text;

public class UI_EnemyInfo : MonoBehaviour
{
    [SerializeField] private Image monsterImg;
    [SerializeField] private TextMeshProUGUI costText; // 공격력
    [SerializeField] private TextMeshProUGUI hpText    ;   // 체력
    [SerializeField] private TextMeshProUGUI speedText ;   // 이동속도
    [SerializeField] private TextMeshProUGUI manualText;

    StringBuilder stringBuilder = new StringBuilder();

    public void Init(EnemySO so)
    {
        monsterImg.sprite = so.sprite;

        hpText.SetText(so.HP.ToString());
        speedText.text = so.moveSpeed.ToString();
        costText.text = so.spawnCost.ToString();
        manualText.text = "고블린에 대한 설명..";

        gameObject.SetActive(false);
    }
}
