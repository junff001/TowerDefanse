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
    private int actIndex = 0; // 기록된 행동 박스에서 index를 추가하며 time을 비교.
    int beforeIdx = 0;

    public Color overlapColor;
    public Text monsterText;

    public bool isWaveProgress = false;
    public float currentTime { get; set; } = 0f;
    float sideLength = 0f;

    public List<UI_CancelActBtn> waitingActs = new List<UI_CancelActBtn>(); // 몹 편성 눌러서 여기에 추가.

    public ActData addedAct = null;
    public UI_CancelActBtn addedBtn = null;
    public UI_CancelActBtn cancleBtnPrefab; // 이걸 누르면 동작을 취소함.

    public RectTransform waitingActContentTrm;
    public RectTransform invisibleObj;

    public UI_AddActBtn draggingBtn = null;


    private void Start()
    {
        sideLength = cancleBtnPrefab.GetComponent<RectTransform>().rect.width;
    }

    private void Update()
    {
        if(isWaveProgress)
        {
            currentTime += Time.deltaTime;

            RecordedSegmentCheck();
        }
    }

    public void InitRecordLoad()
    {
        actIndex = 0;
        currentTime = 0;
        RecordedSegmentCheck();
    }

    public void RecordedSegmentCheck()
    {
        if (Managers.Record.recordBox[WaveManager.Instance.Wave - 1].recordLog.Count > actIndex)
        {
            RecordBase recordSegment = Managers.Record.recordBox[WaveManager.Instance.Wave - 1].recordLog[actIndex];

            if (recordSegment.recordedTime <= currentTime)
            {
                RecordSegmentPlay(recordSegment);

                actIndex++;

                // 같은 시간에 기록될 수 있으니 재귀
                RecordedSegmentCheck();
            }
        }
    }

    public void RecordedSegmentPlayAll()
    {
        int count = Managers.Record.recordBox[WaveManager.Instance.Wave - 1].recordLog.Count;

        for (int i = actIndex; i < count; i++)
        {
            RecordBase recordSegment = Managers.Record.recordBox[WaveManager.Instance.Wave - 1].recordLog[actIndex];
            RecordSegmentPlay(recordSegment);
        }
    }

    private void RecordSegmentPlay(RecordBase recordSegment)
    {
        switch(recordSegment.recordType)
        {
            case eRecordType.TOWER_PLACE:
                RecordTowerPlace segment = recordSegment as RecordTowerPlace;
                BuildManager.Instance.SpawnTower(segment.towerSO, segment.towerPos);

                break;
            case eRecordType.TOWER_UPGRADE:

                break;
        }
    }

    public bool IsSameAct(ActData compareActData, ActData newActData) // 전에 추가한 것과 
    {
        if (waitingActs.Count > 0 && compareActData == null)
            compareActData = waitingActs[waitingActs.Count - 1].actData;

        if (compareActData == null) return false; //처음이면 당연히 새로 추가

        if (compareActData.actType == Define.ActType.Wait) // 전에 추가한게 휴식일 때,
        {
            if (compareActData.actType == newActData.actType) // 새로 추가한것도 휴식이면 같음.
                return true;
            else
                return false;
        }
        else if (newActData.actType == Define.ActType.Enemy)
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

    public IEnumerator SpawnEnemy(Define.MonsterType monsterType)
    {
        waitingActs[0].Cancel();

        EnemyBase enemy = WaveManager.Instance.enemyDic[monsterType];
        EnemyBase enemyObj = Instantiate(enemy, Managers.Game.wayPoints[0].transform.position, enemy.transform.rotation, this.transform);
        HealthSystem enemyHealth = enemyObj.GetComponent<HealthSystem>();
        WaveManager.Instance.aliveEnemies.Add(enemyObj);

        yield return new WaitForSeconds(0.5f); // 스폰 텀

        if (waitingActs.Count == 1 && waitingActs[0].actData.actType == Define.ActType.Wait)
        {
            //남은 행동이 전부 휴식이면 그냥 전부 해제하장
            int count = waitingActs[0].actStackCount;

            for (int i = 0; i < count; i++)
            {
                waitingActs[0].Cancel();
            }
        }
        TryAct();

    }

    public void OnAddAct(ActData actData)
    {
        if(actData.actType == Define.ActType.Enemy)
        {
            curAddedMonsterCount += actData.spawnCost;
        }
        else
        {
            curAddedRestCount++;
        }
        UpdateTexts();
    }
    public void OnCancelAct(ActData actData)
    {
        if (actData.actType == Define.ActType.Enemy)
        {
            curAddedMonsterCount -= actData.spawnCost;
        }
        else
        {
            curAddedRestCount--;
        }
        UpdateTexts();
    }
    public void UpdateTexts()
    {
        monsterText.text = $"{curAddedMonsterCount}/{MaxMonsterCount}";
    }

    public void CheckActType(ActData actData)
    {
        switch (actData.actType)
        {
            case Define.ActType.Wait:
                StartCoroutine(Wait());
                break;

            case Define.ActType.Enemy:
                StartCoroutine(SpawnEnemy(actData.monsterType));
                break;
        }
    }

    public void WaveStart() //TryAct라는 말이 웨이브 시작할 때 실행할 함수명으로 적절치 않아서 그냥 WaveStart라고 따로 만들어뒀어여
    {
        if(!isWaveProgress)
        {
            if (curAddedMonsterCount <= MaxMonsterCount && curAddedMonsterCount > 0) // 아예 안소환하는건 이상하니까.. 0은 체크했습니당
            {
                isWaveProgress = true;
                canAddWave = false;
                TryAct();
            }
            else
            {
                UIManager.SummonText(new Vector2(Screen.width / 2, Screen.height / 2),
                    $"현재 웨이브 수{curAddedMonsterCount}/{MaxMonsterCount}", 60);
            }
        }
        else
        {
            UIManager.SummonText(new Vector2(Screen.width / 2, Screen.height / 2),
                    $"웨이브 진행중입니다!", 60);
        }
    }

    IEnumerator Wait()
    {
        Debug.Log("1초 대기");
        yield return new WaitForSeconds(1f);
        waitingActs[0].Cancel();
        TryAct();
    }

    public bool canAddWave = true;

    void TryAct() // waiting 행동을 그거를 버튼으로 
    {
        if (waitingActs.Count > 0)
        {
            CheckActType(waitingActs[0].actData);
        }
        {
            canAddWave = true;
        }
    }

    public void AddAct(ActData actData)
    {
        ActData newAct = new ActData(actData.actType, actData.monsterType, actData.spawnCost);
        OnAddAct(newAct);
        if (IsSameAct(addedAct, newAct))
        {
            if (addedBtn != null) addedBtn.Stack();
        }
        else // 새로 버튼 추가
        {
            AddBtn(newAct);
        }
    }

    public void InsertAct(Vector3 dragEndPos, ActData actData)
    {
        ActData newAct = new ActData(actData.actType, actData.monsterType, actData.spawnCost);

        OnAddAct(newAct);
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
        foreach (var item in waitingActs) item.cancelActBtn.image.color = Color.white;
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
                waitingActs[0].cancelActBtn.image.color = overlapColor;
            }
        }
        else // 인덱스에 알맞게 투명 이미지 옮겨주기
        {
            SetInvisibleObj(insertIdx + 1);
            if (IsSameAct(waitingActs[insertIdx].actData, newAct)) // 드래그 해서 넣은 곳 기준 왼쪽 버튼
            {
                waitingActs[insertIdx].cancelActBtn.image.color = overlapColor;
            }
            else if (insertIdx + 1 <= waitingActs.Count - 1) // 인덱스 안넘어가도록..
            {
                if (IsSameAct(waitingActs[insertIdx + 1].actData, newAct)) // 왼쪽 버튼 옆의 오른쪽 놈.
                {
                    waitingActs[insertIdx + 1].cancelActBtn.image.color = overlapColor;
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

    public bool CanSetWave(Define.ActType actType)
    {
        if(actType == Define.ActType.Enemy)
        {
            return MaxMonsterCount > curAddedMonsterCount && canAddWave;
        }
        else
        {
            return MaxRestCount > curAddedRestCount && canAddWave;
        }
    }


    public void OnCreateRemoveBtn(ActData newAct, UI_CancelActBtn newBtn)
    {
        newBtn.Init(newAct);
        newBtn.Stack();
        addedBtn = newBtn; // 같은거면 쌓아줘야 하니까 변수에 넣어주고~
        addedAct = newAct;
        RefreshRemoveIdxes();

        newBtn.cancelActBtn.onClick.AddListener(() =>
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
