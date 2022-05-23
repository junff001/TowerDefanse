using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class UIManager : Singleton<UIManager>
{
    public Text hpText;
    public Text moneyText;

    public Button speedButton;

    public UI_TowerUpgradePanel towerUpgradePanel = null;

    public UI_AddActBtn[] waitingActAddBtns;


    private void Awake()
    {
        SetWaitingActAddBtns();
    }

    private void Start()
    {
        UpdateGoldText();
    }

    private void SetWaitingActAddBtns()
    {
        waitingActAddBtns[0].actData = new ActData(ActType.Enemy, MonsterType.Goblin);
        waitingActAddBtns[1].actData = new ActData(ActType.Enemy, MonsterType.Ghost);
        waitingActAddBtns[2].actData = new ActData(ActType.Enemy, MonsterType.Slime);
        waitingActAddBtns[3].actData = new ActData(ActType.Enemy, MonsterType.IronBore);
        waitingActAddBtns[4].actData = new ActData(ActType.Wait);
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
