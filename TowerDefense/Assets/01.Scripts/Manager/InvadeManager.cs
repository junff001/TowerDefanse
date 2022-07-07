using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

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

    public int throwerSpawnCost;
    public int suicideBomberSpawnCost;

    [HideInInspector] public bool isOffenseProgress = false;
    [HideInInspector] public List<UI_SpawnMonster> bookmarkedMonsters = new List<UI_SpawnMonster>();

    [SerializeField] private GameObject towerFocusPanel; // 타워만 잘 보이게 하기 위한 UI 패널
    [SerializeField] private GameObject btnFocusPanel; // 내가 선택한 몬스터가 뭔지 보여주는거.
    [SerializeField] private RectTransform focusBtn;

    private EnemySO enemySO; // 자폭병 소환 때 사용할 친구.

    private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI monsterAttackTypeText; // 폭병 / 자폭병
    [SerializeField] private Button changeBtn; // 처음엔 시작버튼임!
    [SerializeField] private Image spawnAttackTypeIcon; // 폭병/ 자폭병 뭐 소환할지 아이콘

    [SerializeField] private Sprite thrower;
    [SerializeField] private Sprite suicideBomber;
    
    [HideInInspector] public bool isSpawnningThrower = true;
    [HideInInspector] public bool isSelectingTower = false;
    private bool isChanging = false;
    
    private string throwerString = "투척병";
    private string suicideBomberString = "자폭병";

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
                monsterAttackTypeText.text = isSpawnningThrower ? throwerString : suicideBomberString;
                spawnAttackTypeIcon.sprite = thrower;
            }
            else
            {
                if (false == isChanging)
                {
                    isChanging = true;
                    spawnAttackTypeIcon.transform.DORotate(new Vector3(0, -90, 0), 0.5f).OnComplete(() =>
                    {
                        spawnAttackTypeIcon.transform.eulerAngles = new Vector3(0, -270, 0);
                        isSpawnningThrower = !isSpawnningThrower;
                        spawnAttackTypeIcon.sprite = isSpawnningThrower ? thrower : suicideBomber;
                        spawnAttackTypeIcon.transform.DORotate(new Vector3(0, -90, 0), 0.5f).OnComplete(() => 
                        {
                            isChanging = false;
                            monsterAttackTypeText.text = isSpawnningThrower ? throwerString : suicideBomberString;
                        }).SetRelative();
                    }).SetRelative();
                }
            }
        });
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0) && isSelectingTower)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, 30);
            if(hit.collider != null)
            {
                if(hit.transform.CompareTag("Tower"))
                {
                    SpawnEnemy(enemySO, hit.transform);
                }
            }
            isSelectingTower = false;
            SetScreenDark(false, Vector3.zero);
            Managers.Build.map.SetTilemapsColorDark(false);
        }

        if(isOffenseProgress) // 오펜스 시작 버튼 누르면 시작함.
        {
            time -= Time.deltaTime;
            time = Mathf.Clamp(time, 0, maxTime);
            timerText.text = time.ToString("0");


            if (time <= 0) // clamp 걸어놨엉
            {
                isOffenseProgress = false;
                PopupText text = new PopupText("게임오버!");
                StopCoroutine(Managers.Invade.CoGoldPlus());
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

    public IEnumerator CoGoldPlus()
    {
        while(true)
        {
            Managers.Gold.GoldPlus(1);
            yield return new WaitForSeconds(0.01f);
        }
    }

    public void WaveStart() // 오펜스 시작, 타이머 시작, 시작 소리 플레이
    {
        if (isOffenseProgress == false)
        {
            isOffenseProgress = true;
            curSpawnIdx = 0;
            Managers.Sound.Play("System/StartWave");
            StartCoroutine(Managers.Invade.CoGoldPlus());
        }
        else
        {
            Debug.Log("이미 오펜스를 시작했습니다");
        }
    }
    
    public void SetScreenDark(bool on, Vector3 btnFocusPanelMovePos)
    {
        focusBtn.transform.position = new Vector3(btnFocusPanelMovePos.x, focusBtn.transform.position.y, focusBtn.transform.position.z);
        btnFocusPanel.SetActive(on);
        towerFocusPanel.SetActive(on);
    }

    public void SetEnemySO(EnemySO enemySO) => this.enemySO = enemySO;
    public EnemySO GetEnemySO() => this.enemySO;

    public void SpawnEnemy(EnemySO enemySO, Transform target = null)
    {
        int spawnCost = enemySO.spawnCost;
        spawnCost += isSpawnningThrower ? throwerSpawnCost : suicideBomberSpawnCost;

        if(Managers.Gold.GoldMinus(spawnCost))
        {
            int wayCount = Managers.Stage.selectedStage.pointLists.Count; // 경로 갯수
            int firstIdx = Managers.Stage.selectedStage.pointLists[curSpawnIdx].indexWayPoints[0];// 최초로 스폰될 웨이포인트의 인덱스

            Enemy enemyObj = Instantiate(enemySO.basePrefab, Managers.Game.wayPoints[firstIdx].transform.position, enemySO.basePrefab.transform.rotation, this.transform);
            enemyObj.enemyData.InitEnemyData(enemySO, Managers.Game.GetCoefficient().coefEnemyHP / 100);
            enemyObj.healthSystem.livingEntity = enemyObj;
            enemyObj.spineController.Init(enemySO.spineData);
            enemyObj.wayPointListIndex = curSpawnIdx;

            if (target != null) enemyObj.target = target;

            Managers.Wave.aliveEnemies.Add(enemyObj);
            curSpawnCount++;

            if (curSpawnCount > Managers.Wave.mapInfoSO.offenseHeadCount)
            {
                curSpawnCount = 0;
                curSpawnIdx = (curSpawnIdx + 1) % wayCount;
            }
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
