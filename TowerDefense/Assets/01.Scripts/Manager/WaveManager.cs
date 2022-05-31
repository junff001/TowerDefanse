using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public enum eGameMode
{
    DEFENSE,
    OFFENSE
}

public class WaveManager : Singleton<WaveManager>
{
    [Header("Object Field")]
    public Text waveRoundCount;

    [Header("풀매니저 오브젝트")]
    public Transform poolManagerTrm;

    [Header("웨이브")]
    private int _wave = 1;
    public int Wave
    {
        get
        {
            return _wave;
        }
        set
        {
            _wave = value;
            waveRoundCount.text = $"Wave {_wave}";
        }
    }

    public WaveSO waveSO;

    public Dictionary<MonsterType, EnemyBase> enemyDic = new Dictionary<MonsterType, EnemyBase>();
    public List<EnemyBase> enemyList = new List<EnemyBase>();

    public List<EnemyBase> aliveEnemies = new List<EnemyBase>();
    public Queue<EnemyBase> enemySpawnQueue = new Queue<EnemyBase>();

    [Header("디펜스UI")]
    public CanvasGroup defenseTowerGroup;
    public RectTransform defenseStatus;
    public Text defenseHpText;

    [Header("오펜스UI")]
    public CanvasGroup offenseMonsterGroup;
    public RectTransform offenseStatus;
    public Text offenseHpText;

    private eGameMode gameMode;
    [HideInInspector]
    public eGameMode GameMode
    {
        get
        {
            return gameMode;
        }

        set
        {
            gameMode = value;
            ChangeMode(gameMode);
        }
    }
    bool IsWaveProgressing
    {
        get
        {
            return aliveEnemies.Count != 0; // 웨이브가 진행중이면 true
        }
    }

    private void Start()
    {
        DefenseSetNextWave();

        for(int i = 0; i< enemyList.Count; i++)
        {
            enemyList[i].InitEnemyData();
        }

        for (int i = 0; i < enemyList.Count; i++)
        {
            enemyDic.Add(enemyList[i].enemyData.MonsterType, enemyList[i]);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameManager.Instance.UpdateHPText();
            if (GameMode == eGameMode.DEFENSE)
            {
                UIManager.SummonText(new Vector2(960, 300), "디버그 : 오펜스 모드!", 40);
                GameMode = eGameMode.OFFENSE;
            }
            else
            {
                UIManager.SummonText(new Vector2(960, 300), "디버그 : 디펜스 모드!", 40);
                GameMode = eGameMode.DEFENSE;
            }
        }
    }



    public void DefenseSetNextWave()
    {
        RecordManager.Instance.recordBox.Add(new RecordWaveBox());
        SpawnerMonsterCount[] enemyBox = waveSO.waveEnemyInfos[Wave - 1].monsterBox;
        foreach (SpawnerMonsterCount item in enemyBox)
        {
            for (int i = 0; i < item.enemyCount; i++)
            {
                enemySpawnQueue.Enqueue(item.enemy);
            }
        }

        if (waveSO.waveEnemyInfos[Wave - 1].boss != null)
        {
            enemySpawnQueue.Enqueue(waveSO.waveEnemyInfos[Wave - 1].boss);
        }
        // 보스도 있으면 큐에 추가해줘야함
    }

    public void WaveStart()
    {
        if (false == IsWaveProgressing)
        {
            RecordManager.Instance.StartRecord();
            StartCoroutine(Spawn());
        }
    }

    public void OnWaveEnd(int rewardGold, int rewardWave)
    {
        if (gameMode == eGameMode.DEFENSE)
        {
            GoldManager.Instance.GoldPlus(rewardGold);
            UIManager.SummonText(new Vector2(Screen.width / 2, Screen.height / 2), $"{rewardGold} 지급!", 60);
            Debug.Log("돈 추가");

            
        }
        else
        {
            InvadeManager.Instance.MaxMonsterCount += rewardWave;
            UIManager.SummonText(new Vector2(Screen.width / 2, Screen.height / 2), $"웨이브 편성 수 {rewardWave} 증가!", 60);
            Debug.Log("인원 추가");
        }
        InvadeManager.Instance.UpdateTexts();
    }

    public void CheckWaveEnd()
    {
        if (gameMode == eGameMode.DEFENSE)
        {
            //몹이 죽을 때 실행되는 함수
            if (IsWaveProgressing == false && enemySpawnQueue.Count == 0)
            {
                // 여기서 해주면 돼
                RecordManager.Instance.EndRecord();

                // 디펜스 클리어 체크
                if (Wave >= waveSO.waveEnemyInfos.Length)
                {
                    // UI나 컷신같은거 나오고 교체..일걸요?

                    // 오펜스 모드로 교체!
                    GameMode = eGameMode.OFFENSE;
                }
                else
                {
                    // 돈 추가 , 인원추가
                    OnWaveEnd(300, 0);

                    Wave++;
                    DefenseSetNextWave();
                }
            }
        }
        else // 오펜스 모드라면
        {
            if (IsWaveProgressing == false && InvadeManager.Instance.isWaveProgress)
            {
                // 오펜스 클리어 체크
                if (Wave >= waveSO.waveEnemyInfos.Length)
                {
                    // UI나 컷신같은거 나오고 게임 클리어!
                    Debug.Log("게임 클리어");
                }
                else
                {
                    OnWaveEnd(0, 2);
                    InvadeManager.Instance.isWaveProgress = false;
                    InvadeManager.Instance.RecordedSegmentPlayAll();

                    Wave++;
                    InvadeManager.Instance.InitRecordLoad();
                }
            }
        }
    }

    IEnumerator Spawn()
    {
        int queueCount = enemySpawnQueue.Count;
        int count_five = 0;

        for (int i = 0; i < queueCount; i++)
        {
            if (count_five >= 5)
            {
                count_five = 0;
                yield return new WaitForSeconds(1f);
            }

            EnemyBase enemy = enemySpawnQueue.Dequeue();

            EnemyBase enemyObj = Instantiate(enemy, GameManager.Instance.wayPoints[0].transform.position, enemy.transform.rotation, this.transform);
            HealthSystem enemyHealth = enemyObj.GetComponent<HealthSystem>();

            aliveEnemies.Add(enemyObj);

            enemyHealth.OnDied += () =>
            {
                aliveEnemies.Remove(enemyObj);
                CheckWaveEnd();
                Destroy(enemyObj.gameObject);
            };

            count_five++;

            yield return new WaitForSeconds(0.5f);
        }
    }

    private void ChangeMode(eGameMode gameMode)
    {
        switch (gameMode)
        {
            case eGameMode.DEFENSE:
                {
                    if (InvadeManager.Instance.draggingBtn != null)
                    {
                        InvadeManager.Instance.draggingBtn.OnDragEnd();
                    }

                    GameManager.hpText = defenseHpText;
                    defenseStatus.transform.SetAsLastSibling();
                    defenseStatus.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.Linear);
                    offenseStatus.DOAnchorPos(new Vector2(42, 12), 0.3f).SetEase(Ease.Linear);

                    RectTransform towerRect = defenseTowerGroup.GetComponent<RectTransform>();
                    RectTransform monsterRect = offenseMonsterGroup.GetComponent<RectTransform>();

                    CanvasGroupInit(defenseTowerGroup, true);
                    towerRect.DOAnchorPosY(0, 0.5f);

                    CanvasGroupInit(offenseMonsterGroup, false);
                    monsterRect.DOAnchorPosY(-monsterRect.sizeDelta.y, 0.5f);
                }
                break;
            case eGameMode.OFFENSE:
                {
                    if (BuildManager.Instance.movingTowerImg != null)
                    {
                        BuildManager.Instance.movingTowerImg.GetComponent<RectTransform>().anchoredPosition = Vector3.zero; // 돌려보내기
                        BuildManager.Instance.ResetCheckedTiles();
                        BuildManager.Instance.movingTowerImg = null;
                    }

                    Wave = 1;

                    GameManager.hpText = offenseHpText;
                    offenseStatus.transform.SetAsLastSibling();
                    offenseStatus.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.Linear);
                    defenseStatus.DOAnchorPos(new Vector2(42, 12), 0.3f).SetEase(Ease.Linear);

                    RectTransform towerRect = defenseTowerGroup.GetComponent<RectTransform>();
                    RectTransform monsterRect = offenseMonsterGroup.GetComponent<RectTransform>();

                    CanvasGroupInit(offenseMonsterGroup, true);
                    monsterRect.DOAnchorPosY(0, 0.5f);

                    CanvasGroupInit(defenseTowerGroup, false);
                    towerRect.DOAnchorPosY(-towerRect.sizeDelta.y, 0.5f);

                    //타워 지우기
                    foreach (Tower tower in BuildManager.Instance.spawnedTowers)
                    {
                        Destroy(tower.gameObject);
                    }
                    BuildManager.Instance.spawnedTowers.Clear();

                    //공격 전부 꺼주기(Bullet 상속 받은 친구들)
                    Transform[] poolManagerChildren = poolManagerTrm.GetComponentsInChildren<Transform>();
                    Transform[] poolingObjs = new Transform[poolManagerChildren.Length - 1];

                    //맨 위의 PoolManagerObj 제외시키기.
                    for (int i = 1; i < poolManagerChildren.Length; i++)
                    {
                        poolingObjs[i - 1] = poolManagerChildren[i];
                    }

                    for (int i = 0; i < poolingObjs.Length; i++)
                    {
                        poolingObjs[i].gameObject.SetActive(false);
                    }

                    InvadeManager.Instance.InitRecordLoad();
                }
                break;
        }

        GameManager.Instance.UpdateHPText();

        void CanvasGroupInit(CanvasGroup group, bool appear)
        {
            if (appear) group.transform.SetAsLastSibling();

            group.alpha = appear ? 1 : 0.3f;
            group.interactable = appear;
            group.blocksRaycasts = appear;
        }
    }
}
