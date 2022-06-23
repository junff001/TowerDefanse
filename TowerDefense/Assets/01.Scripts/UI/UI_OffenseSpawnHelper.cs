using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_OffenseSpawnHelper : MonoBehaviour
{
    public UI_SpawnMonster spawnBtnPrefab;

    // 고른 탭에 따라 달라질 값입니당.
    public Define.SpeciesType targetSpeciesType = Define.SpeciesType.None;
    public Define.MonsterType targetMonsterType = Define.MonsterType.None;

    public Define.SpeciesType curSpeciesType = Define.SpeciesType.Goblin;
    public Define.MonsterType curMonsterType = Define.MonsterType.Normal;

    public Transform contentTrm;

    List<UI_SpawnMonster> spawnBtnList = new List<UI_SpawnMonster>();
    List<UI_SpawnMonster> activedBtns = new List<UI_SpawnMonster>();

    public Button speciesBtn1;
    public Button speciesBtn2;

    public Button armorBtn;
    public Button flyBtn;
    public Button hideBtn;
    public Button shieldBtn;

    private void Start()
    {
        speciesBtn1.onClick.AddListener(() => SetSpeciesType());
        speciesBtn2.onClick.AddListener(() => SetSpeciesType());

        armorBtn.onClick.AddListener(() => SetMonsterType(Define.MonsterType.Armor));
        flyBtn.onClick.AddListener(() => SetMonsterType(Define.MonsterType.Fly));
        hideBtn.onClick.AddListener(() => SetMonsterType(Define.MonsterType.Hide));
        shieldBtn.onClick.AddListener(() => SetMonsterType(Define.MonsterType.Shield));

        MakeAllBtns();
        ViewBtns();
    }

    public void SetSpeciesType()
    {
        targetSpeciesType = curSpeciesType;
        ViewBtns();
    }

    public void SetMonsterType(Define.MonsterType monsterType)
    {
        if (targetMonsterType != monsterType)
        {
            targetMonsterType = monsterType;
        }
        else
        {
            targetMonsterType = Define.MonsterType.None;
        }
        ViewBtns();
    }


    public void ClearBtns()
    {
        for (int i = 0; i < activedBtns.Count; i++)
        {
            activedBtns[i].gameObject.SetActive(false);
        }
        activedBtns.Clear();
    }

    public void MakeAllBtns() // 다 만들어놓고.
    {
        foreach (Dictionary<Define.MonsterType, EnemySO> dic in Managers.Wave.speciesDic.Values)
        {
            foreach (EnemySO so in dic.Values)
            {
                UI_SpawnMonster newBtn = Instantiate(spawnBtnPrefab, contentTrm);
                newBtn.Init(so);
                newBtn.gameObject.SetActive(false);
                spawnBtnList.Add(newBtn);
            }
        }
    }

    public void ViewBtns() // 조건에 맞게 보여주기
    {
        ClearBtns();

        if (targetSpeciesType != Define.SpeciesType.None)
        {
            List<UI_SpawnMonster> sameSTypeBtns = spawnBtnList.FindAll((x) => x.so.SpeciesType == targetSpeciesType);
            CheckMonsterType(sameSTypeBtns);
        }
        else
        {
            CheckMonsterType(spawnBtnList);
        }


    }

    void CheckMonsterType(List<UI_SpawnMonster> targetList) // 몹 타입이 None인지 체크 후,
    {
        if (targetMonsterType != Define.MonsterType.None) // 정한 몹 타입 있음.
        {
            List<UI_SpawnMonster> sameMTypeBtns = targetList.FindAll(x => Define.HasType(x.so.MonsterType, targetMonsterType));
            TurnOnBtns(sameMTypeBtns);
        }
        else // 없음
        {
            TurnOnBtns(targetList);
        }

        void TurnOnBtns(List<UI_SpawnMonster> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].gameObject.SetActive(true);
                activedBtns.Add(list[i]);
            }
        }
    }
}
