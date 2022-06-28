using UnityEngine;
using TMPro;

public class InvadeManager : MonoBehaviour
{
    public int MaxMonsterCount = 3;
    public int curAddedMonsterCount = 0;
    private int actIndex = 0; // 기록된 행동 박스에서 index를 추가하며 time을 비교.

    private int curSpawnIdx = 0;
    private int curSpawnCount = 0;

    public Color overlapColor;
    public bool isWaveProgress = false;
    public float currentTime { get; set; } = 0f;

    // ____________ 여기까지는 기존 인베이드 매니저에 필요한 데이터

    public float time = 0f;
    public float maxTime = 180f;
    public bool isOffenseProgress = false;

    [SerializeField] private TextMeshProUGUI timerText;

    private void Start()
    {
        timerText = Managers.Wave.roundCountText;
    }

    private void Update()
    {
        if(isOffenseProgress) // 오펜스 시작 버튼 누르면 시작함.
        {
            time += Time.deltaTime;
            timerText.text = Mathf.Clamp(Mathf.FloorToInt(time), 0, maxTime).ToString();

            if (time > maxTime)
            {
                isOffenseProgress = false;
                PopupText text = new PopupText("게임오버!");
                Managers.UI.SummonPosText(Vector2.zero, text);
                Debug.Log("오펜스 실패!");
            }
        }

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
    
    public void WaveStart() // 오펜스 시작, 타이머 시작, 시작 소리 플레이
    {
        if(isOffenseProgress == false)
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

        EnemyBase enemy = so.BasePrefab;
        enemy.InitEnemyData(so, Managers.Game.pctByEnemyHP_Dict_DEF[GameManager.StageLevel] / 100);

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

}
