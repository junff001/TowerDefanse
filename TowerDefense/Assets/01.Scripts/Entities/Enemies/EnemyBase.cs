using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float HP;
    public float Shield;
    public int OffensePower;
    public float MoveSpeed;
    public int RewardGold;
    public bool IsHide;
    public bool IsGuardian;
    public bool IsFlying;
    public PropertyType PropertyResistance;
    public bool IsDebuffIimmune;
    public MonsterType monsterType;
}

public class EnemyBase : MonoBehaviour
{
    protected HealthSystem healthSystem;

    [SerializeField] protected EnemySO enemySO;
    public EnemyData enemyData = new EnemyData();

    private List<BuffBase> buffList = new List<BuffBase>();

    private int currentWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    void Awake()
    {
        InitEnemyData();
        healthSystem = GetComponent<HealthSystem>();
    }

    private void Start()
    {
        healthSystem.OnDied += () =>
        {
            GoldManager.Instance.GoldPlus(enemyData.RewardGold);
        };
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
        movedDistance = aliveTime * enemyData.MoveSpeed;
        Move();

        if (buffList.Count > 0)
        {
            for (int i = buffList.Count - 1; i >= 0; i--)
            {
                if (buffList[i].isEnd)
                {
                    buffList.RemoveAt(i);
                }
                else
                {
                    buffList[i].Update();
                }
            }
        }
    }

    public void AddBuff(BuffBase buff)
    {
        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i] == buff)
            {
                if (Mathf.Abs(buffList[i].amplification - buff.amplification) < 0.1f)
                {
                    if (buffList[i].duration < buff.duration)
                    {
                        buffList[i].duration = buff.duration;
                        return;
                    }
                }
                else
                {
                    if (buffList[i].amplification < buff.amplification)
                    {
                        buffList.RemoveAt(i);
                        break;
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        buffList.Add(buff);
    }

    void InitEnemyData()
    {
        enemyData.HP = enemySO.HP;
        enemyData.Shield = enemySO.Shield;
        enemyData.OffensePower = enemySO.OffensePower;
        enemyData.MoveSpeed = enemySO.MoveSpeed;
        enemyData.RewardGold = enemySO.RewardGold;
        enemyData.IsHide = enemySO.IsHide;
        enemyData.IsGuardian = enemySO.IsGuardian;
        enemyData.IsFlying = enemySO.IsFlying;
        enemyData.PropertyResistance = enemySO.PropertyResistance;
        enemyData.IsDebuffIimmune = enemySO.IsDebuffIimmune;
    }

    void Move()
    {
        if (currentWayPointIndex == GameManager.Instance.wayPoints.Count)
        {
            GameManager.Instance.OnEnemyArrivedLastWaypoint(this);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.wayPoints[currentWayPointIndex].transform.position,
            Time.deltaTime * enemyData.MoveSpeed);

        Vector3 dir = GameManager.Instance.wayPoints[currentWayPointIndex].transform.position - transform.position;

        float absXScale = Mathf.Abs(transform.localScale.x);
        float xScale = dir.x > 0 ? -absXScale : absXScale;
        transform.localScale = new Vector3(xScale, transform.localScale.y, transform.localScale.z);
        

        if (WayPointDistance())
        {
            NextPoint();
        }
    }

    bool WayPointDistance()
    {
        if (Vector2.Distance(GameManager.Instance.wayPoints[currentWayPointIndex].transform.position, transform.position) < 0.01f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void NextPoint()
    {
        currentWayPointIndex += 1;
    }

    public virtual void WaveStatControl(int wave)
    {
        float value_f = (wave * Mathf.Pow(1.5f, 0)) * 100;
        int value = (int)value_f;

        healthSystem.SetHealthAmountMax(value, true); // 체력 조절
    }
}