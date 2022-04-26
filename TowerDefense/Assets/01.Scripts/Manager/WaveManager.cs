using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WaveManager : Singleton<WaveManager>
{
    [Header("Object Field")]
    public Text waveRoundCount;
    public Text waveTimer;
    public RectTransform waveWaitingTimeRect;
    public Text waveWaitingTimeTimer;

    private float totalTime = 0f;
    private float waitingTime = 0f;

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

    public bool isWaitingTime { get; private set; } = false;

    public WaveSO waveSO;

    public List<EnemyBase> aliveEnemies = new List<EnemyBase>();
    public Queue<EnemyBase> enemySpawnQueue = new Queue<EnemyBase>();

    private void Update()
    {
        if (!isWaitingTime)
        {
            totalTime += Time.deltaTime;
            string minute = ((int)totalTime / 60).ToString("00");
            string second = ((int)totalTime % 60).ToString("00");

            waveTimer.text = string.Format("{0}:{1}", minute, second);
        }

        if (waitingTime > 0)
        {
            waitingTime -= Time.deltaTime;
            string wTSec = Mathf.CeilToInt(waitingTime).ToString();
            waveWaitingTimeTimer.text = $"대기 시간\n{wTSec}";

            if (waitingTime < 0)
            {
                // 대기 시간 끝, 적이 나오기 시작
                waveWaitingTimeRect.DOAnchorPosY(waveWaitingTimeRect.sizeDelta.y, 1);
                isWaitingTime = false;
                StartCoroutine(Spawn());
            }
        }

        if (aliveEnemies.Count == 0)
        {
            if (enemySpawnQueue.Count == 0)
            {
                if (!isWaitingTime)
                {
                    // 대기 시간 시작! -> 적 큐 채워줘야함
                    waveWaitingTimeRect.DOAnchorPosY(0, 1);
                    isWaitingTime = true;
                    Wave++;

                    SpawnerMonsterCount[] enemyBox = waveSO.waveEnemyInfos[Wave - 1].monsterBox;
                    foreach (SpawnerMonsterCount item in enemyBox)
                    {
                        for (int i = 0; i < item.enemyCount; i++)
                        {
                            enemySpawnQueue.Enqueue(item.enemy);
                        }
                    }

                    // 보스도 있으면 큐에 추가해줘야함

                    waitingTime = 15f;
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
            if(count_five >= 5)
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
                Destroy(enemyObj.gameObject);
            };

            count_five++;

            yield return new WaitForSeconds(0.2f);
        }
    }

    [ContextMenu("웨이브 대기시간 스킵")]
    public void WaitingSkip()
    {
        if (waitingTime > 0)
        {
            waitingTime = 0.1f;
        }
    }
}
