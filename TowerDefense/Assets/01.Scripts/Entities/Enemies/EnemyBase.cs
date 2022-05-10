using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData
{
    public float HP;
    public float Shield;
    public int OffensePower;
    public float MoveSpeed;
    public bool IsHide;
}

public class EnemyBase : MonoBehaviour
{
    protected HealthSystem healthSystem;

    [SerializeField] protected EnemySO enemySO;
    public EnemyData myStat;

    private List<BuffBase> buffList = new List<BuffBase>();

    private int currentWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    void Awake()
    {
        InitEnemyData();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
        movedDistance = aliveTime * myStat.MoveSpeed;
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
        myStat.HP = enemySO.HP;
        myStat.Shield = enemySO.Shield;
        myStat.OffensePower = enemySO.OffensePower;
        myStat.MoveSpeed = enemySO.MoveSpeed;
        myStat.IsHide = enemySO.IsHide;
    }

    void Move()
    {
        if (currentWayPointIndex == GameManager.Instance.wayPoints.Count)
        {
            GameManager.Instance.OnEnemyArrivedLastWaypoint(this);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.wayPoints[currentWayPointIndex].transform.position,
            Time.deltaTime * myStat.MoveSpeed);

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