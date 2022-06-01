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
    public Define.PropertyType PropertyResistance;
    public bool IsDebuffIimmune;
    public Define.MonsterType MonsterType;
}

public abstract class EnemyBase : MonoBehaviour
{
    protected HealthSystem healthSystem;

    [SerializeField] protected EnemySO enemySO;
    [HideInInspector] public EnemyData enemyData = new EnemyData();

    private List<BuffBase> buffList = new List<BuffBase>();
    private MeshRenderer mesh = null;

    private int currentWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    SpineController animController;

    void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        mesh = GetComponent<MeshRenderer>();
        animController = GetComponent<SpineController>();
    }

    private void Start()
    {
        mesh.sortingLayerName = "Character";
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)enemyData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)enemyData.Shield, true);
        healthSystem.OnDied += () =>
        {
            Managers.Gold.GoldPlus(enemyData.RewardGold);
            animController.Die();
            WaveManager.Instance.aliveEnemies.Remove(this);
            WaveManager.Instance.CheckWaveEnd();
            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<Collider2D>().enabled = false;
            healthSystem.enabled = false;          // 스크립트는 종료, 나중에 풀링할거면 이거 켜는 함수 만들어줘
            enabled = false;                       // ''
            Invoke("Die", 3f); // 선한쌤이 핏자국 남기라고 하셨던 거 같음 시체나..
        };
    }

    public void Die()
    {
        Destroy(this.gameObject);
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

    public void InitEnemyData()
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
        enemyData.MonsterType = enemySO.MonsterType;
    }

    void Move()
    {
        if (currentWayPointIndex == Managers.Game.wayPoints.Count)
        {
            Managers.Game.OnEnemyArrivedLastWaypoint(this);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, Managers.Game.wayPoints[currentWayPointIndex].transform.position,
            Time.deltaTime * enemyData.MoveSpeed);

        Vector3 dir = Managers.Game.wayPoints[currentWayPointIndex].transform.position - transform.position;

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
        if (Vector2.Distance(Managers.Game.wayPoints[currentWayPointIndex].transform.position, transform.position) < 0.01f)
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
}