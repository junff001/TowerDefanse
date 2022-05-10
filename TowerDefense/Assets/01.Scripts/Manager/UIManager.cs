using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    public Text hpText;
    public Text moneyText;

    public Button speedButton;

    public UI_TowerUpgradePanel towerUpgradePanel = null;

    private void Awake()
    {
       // Cursor.visible = false;
    }

    private void Start()
    {
        UpdateGoldText();
    }

    public void UpdateHPText()
    {
        hpText.text = GameManager.Instance.Hp.ToString();
    }

    public void UpdateGoldText()
    {
        moneyText.text = GameManager.Instance.Gold.ToString();
    }
}
