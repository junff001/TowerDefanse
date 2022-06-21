using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_SpawnMonster : MonoBehaviour, IPointerDownHandler
{
    [SerializeField] Text usableStackText = null;

    public ActData actData = null;
    public Image monsterImg; // 버튼 대신에 움직여줄 이미지 
    public int chargeTime = 1;
    public int stackCount;

    public int maxStackCount = 10;

    public Image chargeImg; // 스택이 0일 때, 충전률 보여주기

    private void Update()
    {
        if (stackCount >= maxStackCount) return; // 더 쌓아두면 안돼!

        //bool bShowChargeImg = stackCount == 0 ? true : false;
        //chargeImg.gameObject.SetActive(bShowChargeImg);
    }


    public void Init(Define.MonsterType monsterType, Define.SpeciesType speciesType)
    {
        actData = new ActData(monsterType, speciesType);

        EnemySO enemySO = Managers.Wave.speciesDic[actData.speciesType][actData.monsterType];
        monsterImg.sprite = enemySO.Sprite;
        chargeTime = enemySO.ChargeTime;
        usableStackText.text = stackCount.ToString();

       
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Managers.Invade.SpawnEnemy(actData.speciesType, actData.monsterType);
    }
}
