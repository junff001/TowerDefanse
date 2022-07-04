using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using System.Collections.Generic;

public class InvadeManager : MonoBehaviour
{
    #region 기존 인베이드 매니저에서 쓰던 변수들
    private int MaxMonsterCount = 3;
    private int curAddedMonsterCount = 0;
    private int actIndex = 0; // 기록된 행동 박스에서 index를 추가하며 time을 비교.
    private int curSpawnIdx = 0;
    private int curSpawnCount = 0;
    private Color overlapColor;
    [HideInInspector] public bool isWaveProgress = false;
    private float currentTime { get; set; } = 0f;
    #endregion 

    private float maxTime = 30f;
    private float time = 0f;
    public bool isOffenseProgress = false;

    [HideInInspector] public List<UI_SpawnMonster> bookmarkedMonsters = new List<UI_SpawnMonster>();

    private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI monsterAttackTypeText; // 폭병 / 자폭병
    [SerializeField] private Button changeBtn; // 처음엔 시작버튼임!
    [SerializeField] private Image spawnAttackTypeIcon; // 폭병/ 자폭병 뭐 소환할지 아이콘

    [SerializeField] private Sprite bomber;
    [SerializeField] private Sprite suicideBomber;
    
    private bool bSpawnSuicideBombing = false;
    private bool isChanging = false;


    private void Start()
    {
        maxTime = Managers.Wave.mapInfoSO.limitTime;
        time = maxTime;
        timerText = Managers.Wave.roundCountText;

        changeBtn.onClick.AddListener(() =>
        {
            if(false == isOffenseProgress)
            {
                WaveStart();
                monsterAttackTypeText.text = "공격 타입 전환";
                spawnAttackTypeIcon.sprite = bomber;
            }
            else
            {
                if (false == isChanging)
                {
                    isChanging = true;
                    spawnAttackTypeIcon.transform.DORotate(new Vector3(0, -90, 0), 1f).OnComplete(() =>
                    {
                        spawnAttackTypeIcon.transform.eulerAngles = new Vector3(0, -270, 0);
                        bSpawnSuicideBombing = !bSpawnSuicideBombing;
                        spawnAttackTypeIcon.sprite = bSpawnSuicideBombing ? suicideBomber : bomber;
                        spawnAttackTypeIcon.transform.DORotate(new Vector3(0, -90, 0), 1f).OnComplete(() => isChanging = false).SetRelative();
                    }).SetRelative();
                }
            }
        });
    }

    private void Update()
    {
        if(isOffenseProgress) // 오펜스 시작 버튼 누르면 시작함.
        {
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0, maxTime);
            timerText.text = time.ToString("0.00");


            if (time <= 0) // clamp 걸어놨엉
            {
                isOffenseProgress = false;
                PopupText text = new PopupText("게임오버!");
                text.maxSize = 60;
                text.textColor = Color.red;
                text.moveTime = 1.5f;
                Managers.UI.SummonPosText(Vector2.zero, text, true, () => Managers.Game.gameOverUI.gameObject.SetActive(true));
                
            }
        }

        #region 안 쓰는 코드
        /*
        if (isWaveProgress)
        {
            currentTime += Time.deltaTime;
            RecordedSegmentCheck();
        }
        */
        #endregion
    }

    public void WaveStart() // 오펜스 시작, 타이머 시작, 시작 소리 플레이
    {
        if (isOffenseProgress == false)
        {
            isOffenseProgress = true;
            curSpawnIdx = 0;
            Managers.Sound.Play("System/StartWave");
        }
        else
        {
            Debug.Log("이미 오펜스를 시작했습니다");
        }
    }

    public void SpawnEnemy(EnemySO so)
    {
        int wayCount = Managers.Stage.selectedStage.pointLists.Count; // 경로 갯수
        int firstIdx = Managers.Stage.selectedStage.pointLists[curSpawnIdx].indexWayPoints[0];// 최초로 스폰될 웨이포인트의 인덱스

        Enemy enemyObj = Instantiate(so.basePrefab, Managers.Game.wayPoints[firstIdx].transform.position, so.basePrefab.transform.rotation, this.transform);
        enemyObj.enemyData.InitEnemyData(so, Managers.Game.GetCoefficient().coefEnemyHP / 100);
        enemyObj.spineController.Init(so.spineData);

        enemyObj.wayPointListIndex = curSpawnIdx;

        Managers.Wave.aliveEnemies.Add(enemyObj);
        curSpawnCount++;

        if (curSpawnCount > Managers.Wave.mapInfoSO.offenseHeadCount)
        {
            curSpawnCount = 0;
            curSpawnIdx = (curSpawnIdx + 1) % wayCount;
        }
    }


    #region 기존 인베이드 매니저 레코드 함수들
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
    #endregion


}
