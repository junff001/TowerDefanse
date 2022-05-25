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

    [Header("웨이브")]
    private int _wave = 0;
    private int Wave
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
        SetNextWave();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
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


    public void SetNextWave()
    {
        if (enemySpawnQueue.Count == 0) // 다음 웨이브가 있으면,
        {
            Wave++;
            
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
        else
        {
            //체력이 0보다 크면 스테이지 클리어
            Debug.Log("스테이지 클리어");
        }
    }

    public void WaveStart()
    {
        if(false == IsWaveProgressing)
        {
            RecordManager.Instance.StartRecord();
            StartCoroutine(Spawn());
        }
    }

    public void CheckWaveEnd()
    {
        //몹이 죽을 때 실행되는 함수
        if(IsWaveProgressing == false && enemySpawnQueue.Count == 0) 
        {
            // 여기서 해주면 돼
            RecordManager.Instance.EndRecord();

            // 웨이크 클리어 체크
            if (Wave >= waveSO.waveEnemyInfos.Length)
            {
                // 오펜스 모드로 교체!
                GameMode = eGameMode.OFFENSE;
            }
            else
            {
                SetNextWave();
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

            enemyObj.WaveStatControl(Wave);
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
