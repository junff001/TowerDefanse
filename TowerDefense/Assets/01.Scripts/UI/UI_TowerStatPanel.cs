using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UI_TowerStatPanel : MonoBehaviour
{
    public TextMeshProUGUI towerName;
    public TextMeshProUGUI towerCost;
    public TextMeshProUGUI towerHP;
    public TextMeshProUGUI towerATKSpeed;
    public TextMeshProUGUI towerATKDamage;
    public TextMeshProUGUI towerATKRange;
    public TextMeshProUGUI characteristic_1_ExPlanation;

    [SerializeField] private Image towerImage;
    [SerializeField] private Image characteristic_1_Icon;
    [SerializeField] private Image characteristic_2_Icon;

    public void InitTowerStat(TowerSO towerSO)
    {
        towerCost.text =        "Cost : " +           towerSO.PlaceCost.ToString();
        towerHP.text =          "HP : " +             towerSO.HP.ToString();
        towerATKSpeed.text =    "Attack Speed : " +   towerSO.AttackSpeed.ToString();
        towerATKDamage.text =   "Attack Damage : " +  towerSO.AttackPower.ToString();
        towerATKRange.text =    "Attack Range : " +   towerSO.AttackRange.ToString();
        towerName.text =                              towerSO.Name;
        towerImage.sprite =                           towerSO.TowerImage;
        characteristic_1_Icon.sprite =                towerSO.CharacteristicIcon;
    }
}
