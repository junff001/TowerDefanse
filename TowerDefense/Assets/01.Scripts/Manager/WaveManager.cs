using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System.Linq;

public class WaveManager : MonoBehaviour
{
    [Header("Object Field")]
    public Text waveRoundCount;

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
            WaveAnim();
        }
    }
    public RectTransform waveRect;
    public WaveSO waveSO;

    public Dictionary<Define.SpeciesType, EnemyBase> basePrefabDict = new Dictionary<Define.SpeciesType, EnemyBase>();

    [Header("종족별로 베이스 되는 친구 하나씩만 넣어줘")]
    public List<EnemyBase> basePrefabList = new List<EnemyBase>();

    public Dictionary<Define.SpeciesType, Dictionary<Define.MonsterType, EnemySO>> speciesDic = 
        new Dictionary<Define.SpeciesType, Dictionary<Define.MonsterType, EnemySO>>();

    [HideInInspector] public List<EnemyBase> aliveEnemies = new List<EnemyBase>();
    public Queue<SpawnerMonsterCount> enemySpawnQueue = new Queue<SpawnerMonsterCount>();

    [Header("디펜스UI")]
    public CanvasGroup defenseTowerGroup;
    public RectTransform defenseStatus;
    public Text defenseHpText;

    [Header("오펜스UI")]
    public CanvasGroup offenseMonsterGroup;
    public RectTransform offenseStatus;
    public Text offenseHpText;

    private Define.GameMode gameMode;
    [HideInInspector]
    public Define.GameMode GameMode
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
    public bool IsWaveProgressing
    {
        get
        {
            return aliveEnemies.Count != 0; // 웨이브가 진행중이면 true
        }
    }

    private void Start()
    {
        int speciesCount = System.Enum.GetValues(typeof(Define.SpeciesType)).Length;

        for(int i = 0; i< basePrefabList.Count; i++)
        {
            basePrefabDict.Add((Define.SpeciesType)System.Enum.Parse(typeof(Define.SpeciesType), basePrefabList[i].name), basePrefabList[i]);
        }

        for (int i = 1; i < speciesCount; i++) // 0번은 None 나와용
        {
            Dictionary<Define.MonsterType, EnemySO> enemyDic = new Dictionary<Define.MonsterType, EnemySO>();
            List<EnemySO> enemySO = Resources.LoadAll<EnemySO>("EnemySOs/" + ((Define.SpeciesType)i).ToString()).ToList(); 
           
            for (int j = 0; j < enemySO.Count; j++) 
            {
                enemyDic.Add(enemySO[j].MonsterType, enemySO[j]);
            }
            speciesDic.Add((Define.SpeciesType)i, enemyDic);
        }

        DefenseSetNextWave();
    }   

    public void DefenseSetNextWave()
    {
        Managers.Record.recordBox.Add(new RecordWaveBox());
        SpawnerMonsterCount[] enemyBox = waveSO.waveEnemyInfos[Wave - 1].monsterBox;
        foreach (SpawnerMonsterCount item in enemyBox)
        {
            for (int i = 0; i < item.enemyCount; i++)
            {
                enemySpawnQueue.Enqueue(item);
            }
        }
    }

    public void WaveStart()
    {
        if (false == IsWaveProgressing)
        {
            Managers.Record.StartRecord();
            StartCoroutine(Spawn());
            Managers.Sound.Play("System/StartWave");
        }
    }

    public void OnWaveEnd(int rewardGold, int rewardWave)
    {
        PopupText text = new PopupText();
        text.maxSize = 60;

        if (gameMode == Define.GameMode.DEFENSE)
        {
            Managers.Gold.GoldPlus(rewardGold);
            text.text = $"{rewardGold} 지급!";
            Managers.UI.SummonRectText(new Vector2(Screen.width / 2, Screen.height / 2), text);

        }
        else
        {
            Managers.Invade.MaxMonsterCount += rewardWave;
            text.text = $"웨이브 편성 수 {rewardWave} 증가!";
            Managers.UI.SummonRectText(new Vector2(Screen.width / 2, Screen.height / 2), text);
        }
    }

    public void CheckWaveEnd()
    {
        if (gameMode == Define.GameMode.DEFENSE)
        {
            //몹이 죽을 때 실행되는 함수
            if (IsWaveProgressing == false && enemySpawnQueue.Count == 0)
            {
                // 여기서 해주면 돼
                Managers.Record.EndRecord();

                // 디펜스 클리어 체크
                if (Wave >= waveSO.waveEnemyInfos.Length)
                {
                    // 오펜스 모드로 교체!
                    GameMode = Define.GameMode.OFFENSE;
                    Managers.Game.towerInfoUI.CloseInfo();

                }
                else
                {
                    // 돈 추가 , 인원추가
                    OnWaveEnd(100, 0);

                    Wave++;
                    DefenseSetNextWave();
                }
            }
        }
        else // 오펜스 모드라면
        {
            if (IsWaveProgressing == false && Managers.Invade.isWaveProgress)
            {
                // 오펜스 클리어 체크
                if (Wave >= waveSO.waveEnemyInfos.Length)
                {
                    Managers.Sound.Play("System/Win");
                    Managers.Game.clearUI.gameObject.SetActive(true);
                    //Time.timeScale = 0;
                }
                else
                {
                    OnWaveEnd(0, 3);
                    Managers.Invade.isWaveProgress = false;
                    Managers.Invade.RecordedSegmentPlayAll();

                    Wave++;
                    Managers.Invade.InitRecordLoad();
                }
            }
        }
    }

    IEnumerator Spawn()
    {
        int queueCount = enemySpawnQueue.Count;

        for (int i = 0; i < queueCount; i++)
        {
            SpawnerMonsterCount enemyInfo = enemySpawnQueue.Dequeue();
            int index = Managers.Game.pointLists[enemyInfo.wayPointListIndex].indexWayPoints[0];

            EnemySO enemySO = speciesDic[enemyInfo.speciesType][enemyInfo.monsterType];
            EnemyBase enemyObj = Instantiate(enemyInfo.enemy, Managers.Game.wayPoints[index].transform.position,
                enemyInfo.enemy.transform.rotation, this.transform);
            enemyObj.InitEnemyData(enemySO);

            enemyObj.wayPointListIndex = enemyInfo.wayPointListIndex;
            aliveEnemies.Add(enemyObj);

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void ChangeMode(Define.GameMode gameMode)
    {
        switch (gameMode)
        {
            case Define.GameMode.DEFENSE:
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
            case Define.GameMode.OFFENSE:
                {
                    if (Managers.Build.movingObj != null)
                    {
                        Managers.Build.movingObj.GetComponent<RectTransform>().anchoredPosition = Vector3.zero; // 돌려보내기
                        Managers.Build.ResetCheckedTiles(true);
                        Managers.Build.movingObj = null;
                    }

                    Wave = 1;
                    Managers.Game.Hp = Managers.Game.maxHp; // 웨이브 전환시..
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
                    foreach (Tower tower in Managers.Build.spawnedTowers)
                    {
                        Destroy(tower.gameObject);
                    }
                    Managers.Build.spawnedTowers.Clear();

                    //공격 전부 꺼주기(Bullet 상속 받은 친구들)
                    Transform[] poolManagerChildren = Managers.Pool.poolInitPos.GetComponentsInChildren<Transform>();
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

                    Managers.Invade.InitRecordLoad();
                }
                break;
        }

        Managers.Game.UpdateHPText();

        void CanvasGroupInit(CanvasGroup group, bool appear)
        {
            if (appear) group.transform.SetAsLastSibling();

            group.alpha = appear ? 1 : 0.3f;
            group.interactable = appear;
            group.blocksRaycasts = appear;
        }
    }

    private void WaveAnim()
    {
        waveRect.DOKill();
        waveRect.DOAnchorPosY(100, 0.75f).SetEase(Ease.InOutBack).OnComplete(() =>
        {
            waveRoundCount.text = $"Wave {_wave}";
            waveRect.DOAnchorPosY(-6, 0.75f).SetEase(Ease.InOutBack);
        });
    }
}
