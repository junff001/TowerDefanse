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

    private float totalTime = 0f;

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

    private void Update()
    {
        WaveStart();
        SetTotalTime();
    }

    bool IsWaveProgressing() => aliveEnemies.Count != 0; // 웨이브가 진행중이면 true

    public void SetTotalTime()
    {
        if (IsWaveProgressing())
        {
            totalTime += Time.deltaTime;
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
        if(IsWaveProgressing() == false)
        {
            SetNextWave();
            StartCoroutine(Spawn());
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
                Destroy(enemyObj.gameObject);
            };

            count_five++;

            yield return new WaitForSeconds(0.2f);
        }
    }
}
