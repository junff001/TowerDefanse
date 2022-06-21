using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
public class InvadeManager : MonoBehaviour
{
    public int MaxMonsterCount = 3;
    public int curAddedMonsterCount = 0;
    private int actIndex = 0; // 기록된 행동 박스에서 index를 추가하며 time을 비교.

    private int curSpawnIdx = 0;
    private int curSpawnCount = 0;

    public Color overlapColor;
    public Text monsterText;

    public bool isWaveProgress = false;
    public float currentTime { get; set; } = 0f;

    public ActData addedAct = null;

    public RectTransform waitingActContentTrm;
    public RectTransform invisibleObj;

    public UI_SpawnMonster draggingBtn = null;


    private void Update()
    {
        if(isWaveProgress)
        {
            currentTime += Time.deltaTime;

            RecordedSegmentCheck();
        }
#if UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space) && Managers.Wave.GameMode == Define.GameMode.OFFENSE)
        {
            WaveStart();
        }
#endif
    }

    public void InitRecordLoad()
    {
        actIndex = 0;
        currentTime = 0;
        RecordedSegmentCheck();
    }

    public void RecordedSegmentCheck()
    {
        if (Managers.Record.recordBox[Managers.Wave.Wave - 1].recordLog.Count > actIndex)
        {
            RecordBase recordSegment = Managers.Record.recordBox[Managers.Wave.Wave - 1].recordLog[actIndex];

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
        int count = Managers.Record.recordBox[Managers.Wave.Wave - 1].recordLog.Count;

        for (int i = actIndex; i < count; i++)
        {
            RecordBase recordSegment = Managers.Record.recordBox[Managers.Wave.Wave - 1].recordLog[actIndex];
            RecordSegmentPlay(recordSegment);
        }
    }

    private void RecordSegmentPlay(RecordBase recordSegment)
    {
        switch(recordSegment.recordType)
        {
            case eRecordType.TOWER_PLACE:
                
                break;
            case eRecordType.TOWER_UPGRADE:

                break;
        }
    }



    public void SpawnEnemy(Define.SpeciesType speciesType, Define.MonsterType monsterType)
    {
        int wayCount = Managers.Stage.selectedStage.pointLists.Count; // 경로 갯수
        int firstIdx = Managers.Stage.selectedStage.pointLists[curSpawnIdx].indexWayPoints[0];// 최초로 스폰될 웨이포인트의 인덱스

        EnemySO enemySo = Managers.Wave.speciesDic[speciesType][monsterType];
        EnemyBase enemy = Managers.Wave.basePrefabDict[speciesType];
        enemy.InitEnemyData(enemySo);

        EnemyBase enemyObj = Instantiate(enemy, Managers.Game.wayPoints[firstIdx].transform.position, enemy.transform.rotation, this.transform);
        enemyObj.wayPointListIndex = curSpawnIdx;

        Managers.Wave.aliveEnemies.Add(enemyObj);
        curSpawnCount++;

        if (curSpawnCount > Managers.Wave.waveSO.offenseHeadCount)
        {
            curSpawnCount = 0;
            curSpawnIdx = (curSpawnIdx + 1) % wayCount;
        }
    }

    public void WaveStart() //TryAct라는 말이 웨이브 시작할 때 실행할 함수명으로 적절치 않아서 그냥 WaveStart라고 따로 만들어뒀어여
    {
        if(!isWaveProgress)
        {
            if (curAddedMonsterCount <= MaxMonsterCount && curAddedMonsterCount > 0) // 아예 안소환하는건 이상하니까.. 0은 체크했습니당
            {
                isWaveProgress = true;

                curSpawnCount = 0;
                curSpawnIdx = 0;

                Managers.Sound.Play("System/StartWave");
            }
            else
            {
                PopupText text = new PopupText($"현재 웨이브 수{curAddedMonsterCount}/{MaxMonsterCount}");
                text.maxSize = 60;

                Managers.UI.SummonRectText(new Vector2(Screen.width / 2, Screen.height / 2), text);
            }
        }
        else
        {
            PopupText text = new PopupText($"웨이브 진행중입니다!");
            text.maxSize = 60;

            Managers.UI.SummonRectText(new Vector2(Screen.width / 2, Screen.height / 2), text);
        }
    }

}
