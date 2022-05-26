using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class InvadeManager : Singleton<InvadeManager>
{
    private readonly int MaxRestCount = 5;
    private int curAddedRestCount = 0;

    public int MaxMonsterCount = 5;
    public int curAddedMonsterCount = 0;
    

    public Color overlapColor;
    public Text hpText;
    public Text monsterText;


    public List<UI_CancelActBtn> waitingActs = new List<UI_CancelActBtn>(); // 몹 편성 눌러서 여기에 추가.

    public ActData addedAct = null;
    public UI_CancelActBtn addedBtn = null;
    public UI_CancelActBtn cancleBtnPrefab; // 이걸 누르면 동작을 취소함.

    public RectTransform waitingActContentTrm;
    public RectTransform invisibleObj;

    public UI_AddActBtn draggingBtn = null;

    int beforeIdx = 0;
    float sideLength = 0f;

    private void Start()
    {
        sideLength = cancleBtnPrefab.GetComponent<RectTransform>().rect.width;
    }

    public bool IsSameAct(ActData compareActData, ActData newActData) // 전에 추가한 것과 
    {
        if (waitingActs.Count > 0 && compareActData == null)
            compareActData = waitingActs[waitingActs.Count - 1].actData;

        if (compareActData == null) return false; //처음이면 당연히 새로 추가

        if (compareActData.actType == ActType.Wait) // 전에 추가한게 휴식일 때,
        {
            if (compareActData.actType == newActData.actType) // 새로 추가한것도 휴식이면 같음.
                return true;
            else
                return false;
        }
        else if (newActData.actType == ActType.Enemy)
        {
            if (compareActData.monsterType == newActData.monsterType) // 둘의 소환하는 몬스터 데이터가 같으면 같음
                return true;
            else
                return false;
        }
        else
        {
            return false; // 여기는.. 그냥 None이니까 있을리 없음!
        }
    }

    int spawnedCount = 0;

    public IEnumerator SpawnEnemy(MonsterType monsterType)
    {
        if (spawnedCount % 5 == 0 && spawnedCount != 0)
        {
            yield return new WaitForSeconds(1f);
        }

        EnemyBase enemy = WaveManager.Instance.enemyDic[monsterType];
        EnemyBase enemyObj = Instantiate(enemy, GameManager.Instance.wayPoints[0].transform.position, enemy.transform.rotation, this.transform);
        HealthSystem enemyHealth = enemyObj.GetComponent<HealthSystem>();
        enemyObj.WaveStatControl(WaveManager.Instance.Wave);
        WaveManager.Instance.aliveEnemies.Add(enemyObj);

        enemyHealth.OnDied += () =>
        {
            WaveManager.Instance.aliveEnemies.Remove(enemyObj);
            WaveManager.Instance.CheckWaveEnd();
            Destroy(enemyObj.gameObject);
        };

        spawnedCount++;

        yield return new WaitForSeconds(0.2f);
        TryAct();
    }

    public void OnAddAct(ActType actType)
    {
        if(actType == ActType.Enemy)
        {
            curAddedMonsterCount++;
        }
        else
        {
            curAddedRestCount++;
        }

        monsterText.text = $"{curAddedMonsterCount}/{MaxMonsterCount}";
    }

    public void OnCancelAct(ActType actType)
    {
        if (actType == ActType.Enemy)
        {
            curAddedMonsterCount--;
        }
        else
        {
            curAddedRestCount--;
        }
        monsterText.text = $"{curAddedMonsterCount}/{MaxMonsterCount}";
    }

    public void CheckActType(ActData actData)
    {
        switch (actData.actType)
        {
            case ActType.Wait:
                StartCoroutine(Wait());
                break;

            case ActType.Enemy:
                StartCoroutine(SpawnEnemy(actData.monsterType));
                break;
        }
    }

    public void WaveStart() //TryAct라는 말이 웨이브 시작할 때 실행할 함수명으로 적절치 않아서 그냥 WaveStart라고 따로 만들어뒀어여
    {
        if(curAddedMonsterCount == MaxMonsterCount)
        {
            TryAct();
        }
        else
        {
            UIManager.SummonText(new Vector2(Screen.width / 2, Screen.height / 2),
                $"현재 웨이브 수{curAddedMonsterCount}/{MaxMonsterCount}", 60);
        }

    }

    IEnumerator Wait()
    {
        Debug.Log("1초 대기");
        yield return new WaitForSeconds(1f);

        TryAct();
    }

    void TryAct() // waiting 행동을 그거를 버튼으로 
    {
        if (waitingActs.Count > 0)
        {
            CheckActType(waitingActs[0].actData);
            waitingActs[0].Cancel();
        }
    }

    public void AddAct(ActType actType, MonsterType monsterType)
    {
        OnAddAct(actType);
        ActData newAct = new ActData(actType, monsterType);

        if (IsSameAct(addedAct, newAct))
        {
            if (addedBtn != null) addedBtn.Stack();
        }
        else // 새로 버튼 추가
        {
            AddBtn(newAct);
        }
    }

    public void InsertAct(Vector3 dragEndPos, ActType actType, MonsterType monsterType)
    {
        OnAddAct(actType);
        ActData newAct = new ActData(actType, monsterType);
        int insertIdx = GetInsertIndex(dragEndPos);

        if (insertIdx == -2) // 추가한게 없으면
        {
            AddBtn(newAct);
        }

        if (insertIdx == -1) // 맨 왼쪽이면.
        {
            if (IsSameAct(waitingActs[0].actData, newAct))
            {
                waitingActs[0].Stack();
            }
            else
            {
                InsertBtn(newAct, 0);
            }
        }
        else if (insertIdx >= 0)// 0이상일 때
        {
            if (IsSameAct(waitingActs[insertIdx].actData, newAct)) // 드래그 해서 넣은 곳 기준 왼쪽 버튼
            {
                waitingActs[insertIdx].Stack();
            }
            else if (insertIdx + 1 <= waitingActs.Count - 1) // 인덱스 안넘어가도록..
            {
                if (IsSameAct(waitingActs[insertIdx + 1].actData, newAct)) // 왼쪽 버튼 옆의 오른쪽 놈.
                {
                    waitingActs[insertIdx + 1].Stack();
                }
                else
                {
                    InsertBtn(newAct, insertIdx + 1);
                }
            }
            else
            {
                InsertBtn(newAct, insertIdx + 1);
            }
        }
        addedAct = newAct;
    }

    public void ResetButtons()
    {
        foreach (var item in waitingActs) item.cancleActBtn.image.color = Color.white;
    }

    public void ShowInsertPlace(Vector3 dragEndPos, ActData newAct)
    {
        int insertIdx = GetInsertIndex(dragEndPos);

        if (beforeIdx == insertIdx) return; // 동일 위치면 버벅거리는 문제 해결

        ResetButtons();

        invisibleObj.DOKill();
        invisibleObj.sizeDelta = new Vector2(0, sideLength);

        if (insertIdx < 0) // 맨 왼쪽
        {
            SetInvisibleObj(0);
            if (waitingActs.Count > 0 && IsSameAct(waitingActs[0].actData, newAct))
            {
                waitingActs[0].cancleActBtn.image.color = overlapColor;
            }
        }
        else // 인덱스에 알맞게 투명 이미지 옮겨주기
        {
            SetInvisibleObj(insertIdx + 1);
            if (IsSameAct(waitingActs[insertIdx].actData, newAct)) // 드래그 해서 넣은 곳 기준 왼쪽 버튼
            {
                waitingActs[insertIdx].cancleActBtn.image.color = overlapColor;
            }
            else if (insertIdx + 1 <= waitingActs.Count - 1) // 인덱스 안넘어가도록..
            {
                if (IsSameAct(waitingActs[insertIdx + 1].actData, newAct)) // 왼쪽 버튼 옆의 오른쪽 놈.
                {
                    waitingActs[insertIdx + 1].cancleActBtn.image.color = overlapColor;
                }
            }
        }
        beforeIdx = insertIdx;
    }

    void SetInvisibleObj(int insertIdx)
    {
        invisibleObj.transform.SetSiblingIndex(insertIdx);
        invisibleObj.DOSizeDelta(new Vector2(sideLength, sideLength), 0.5f);
    }

    public void ReduceDummyObj() // 인서트 종료시 다시 줄여주기
    {
        beforeIdx = -18;
        invisibleObj.DOKill();
        invisibleObj.sizeDelta = new Vector2(0, sideLength);
        invisibleObj.gameObject.SetActive(false);
    }

    public void AddBtn(ActData newAct)
    {
        UI_CancelActBtn newBtn = Instantiate(cancleBtnPrefab, waitingActContentTrm);
        waitingActs.Add(newBtn);
        OnCreateRemoveBtn(newAct, newBtn);
    }

    public void InsertBtn(ActData newAct, int idx)
    {
        UI_CancelActBtn newBtn = Instantiate(cancleBtnPrefab, waitingActContentTrm);
        waitingActs.Insert(idx, newBtn);
        OnCreateRemoveBtn(newAct, newBtn);
        newBtn.transform.SetSiblingIndex(idx);
    }

    public bool CanSetWave(ActType actType)
    {
        if(actType == ActType.Enemy)
        {
            return MaxMonsterCount > curAddedMonsterCount;
        }
        else
        {
            return MaxRestCount > curAddedRestCount;
        }
    }


    public void OnCreateRemoveBtn(ActData newAct, UI_CancelActBtn newBtn)
    {
        newBtn.Init(newAct);
        newBtn.Stack();
        addedBtn = newBtn; // 같은거면 쌓아줘야 하니까 변수에 넣어주고~
        addedAct = newAct;
        RefreshRemoveIdxes();

        newBtn.cancleActBtn.onClick.AddListener(() =>
        {
            newBtn.Cancel();
            RefreshRemoveIdxes();
        });
    }

    public void OnBtnRemoved(int idx)
    {
        if (idx == 0) // 맨 왼쪽
        {
            return;
        }
        else if (waitingActs.Count > idx) //내가 삭제한게 맨 오른쪽 끝이 아님.
        {
            if (IsSameAct(waitingActs[idx - 1].actData, waitingActs[idx].actData))
            {
                for (int i = 0; i < waitingActs[idx].actStackCount; i++)
                {
                    waitingActs[idx - 1].Stack();
                }
                Destroy(waitingActs[idx].gameObject);
                waitingActs.Remove(waitingActs[idx]);

                addedAct = waitingActs[waitingActs.Count - 1].actData;
                addedBtn = waitingActs[waitingActs.Count - 1];
            }
        }
    }

    public void RefreshRemoveIdxes()
    {
        for (int i = 0; i < waitingActs.Count; i++)
        {
            waitingActs[i].idx = i;
        }
    }

    public int GetInsertIndex(Vector3 dragEndPos)
    {
        List<UI_CancelActBtn> copiedList = waitingActs.ToList(); // 값 복사 

        // 드래그 종료된 위치와 가까운 순서대로 정렬, 가장 가까운데, dragendPos - 가까운놈 한 벡터가 +여야 함.
        copiedList.Sort(
            (x, y) => Vector3.Distance(x.transform.position, dragEndPos).CompareTo(
                      Vector3.Distance(y.transform.position, dragEndPos)));

        if (copiedList.Count > 0)
        {
            float x = dragEndPos.x - copiedList[0].transform.position.x;  // 내 마우스 위치 - 리스트의 첫번째 UI 위치
            if (copiedList[0].idx == 0 && x < 0) // 맨 왼쪽
            {
                return -1;
            }
        }

        foreach (var item in copiedList)
        {
            if (dragEndPos.x - item.transform.position.x > 0)
            {
                return item.idx;
            }
        }

        return -2; // list가 Null인 경우
    }
}
