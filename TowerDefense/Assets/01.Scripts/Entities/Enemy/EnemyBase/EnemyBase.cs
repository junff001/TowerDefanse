using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

[System.Serializable]
public class EnemyData
{
    public int OffensePower;
    public int RewardGold;
    public float HP;
    public float Shield;
    public float MoveSpeed;
    public MonsterType MonsterType;
    public SpeciesType SpeciesType;

    public bool IsHide     = false;
    public bool isShield   = false;
    public bool IsArmor    = false;
    public bool IsFly      = false;
}

public abstract class EnemyBase : MonoBehaviour, IInitializable
{
    public HealthSystem healthSystem;
    public EnemyData enemyData = new EnemyData();
    public SpineController sc;

    private List<BuffBase> buffList = new List<BuffBase>();
    private MeshRenderer mesh = null;

    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    public int CurrentWayPointIndex { get => currentWayPointIndex; private set { } }
    private int realWayPointIndex = 0;

    public float aliveTime { get; set; }
    public float movedDistance { get; set; }

    public bool IsDead => healthSystem.IsDead();

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        mesh = GetComponent<MeshRenderer>();
        sc = GetComponent<SpineController>();
    }

    protected virtual void Start()
    {
        mesh.sortingLayerName = "Character";
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)enemyData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)enemyData.Shield, true);
        healthSystem.OnDied += () =>
        {
            Managers.Gold.GoldPlus(enemyData.RewardGold);
            sc.Die();
            Managers.Wave.aliveEnemies.Remove(this);
            Managers.Wave.CheckWaveEnd();
            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<Collider2D>().enabled = false;
            Invoke("Die", 3f); // 선한쌤이 핏자국 남기라고 하셨던 거 같음 시체나..
        };
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }

    protected virtual void Update()
    {
        aliveTime += Time.deltaTime;
        movedDistance = aliveTime * enemyData.MoveSpeed;
        Move();
        CheckBuffs();
    }

    public void CheckBuffs()
    {
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
        //if (buff.buffType == Define.BuffType.DEBUFF && enemyData.IsDebuffIimmune) return;
        if (buff == null) return;

        for (int i = 0; i < buffList.Count; i++)
        {
            if (buffList[i].GetType() == buff.GetType())
            {
                if (Mathf.Abs(buffList[i].amplification - buff.amplification) < 0.01f)
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

    public void InitEnemyData(EnemySO enemySO, float addPercentEnemyHP)
    {
        enemyData.HP            = enemySO.HP;
        enemyData.Shield        = enemySO.Shield;
        enemyData.OffensePower  = enemySO.OffensePower;
        enemyData.MoveSpeed     = enemySO.MoveSpeed;
        enemyData.RewardGold    = enemySO.RewardGold;
        enemyData.MonsterType   = enemySO.MonsterType;
        enemyData.SpeciesType   = enemySO.SpeciesType;
        enemyData.IsHide        = enemyData.MonsterType.HasFlag(MonsterType.Hide);
        enemyData.isShield      = enemyData.MonsterType.HasFlag(MonsterType.Shield);
        enemyData.IsArmor       = enemyData.MonsterType.HasFlag(MonsterType.Armor);
        enemyData.IsFly         = enemyData.MonsterType.HasFlag(MonsterType.Fly);

        enemyData.HP += enemyData.HP * addPercentEnemyHP;
    }

    public void InitAnimController()
    {
        this.gameObject.layer = LayerMask.NameToLayer(enemyData.MonsterType.ToString() + "Enemy");
        if (enemyData.IsHide) // 은신 체크
        {
            sc.skeleton.A = 0.5f;
        }
    }

    void Move()
    {
        if (IsDead) return;

        if (currentWayPointIndex == Managers.Game.GetWaypointCount(wayPointListIndex))
        {
            Managers.Game.OnEnemyArrivedLastWaypoint(this);
            return;
        }

        realWayPointIndex = Managers.Game.pointLists[wayPointListIndex].indexWayPoints[currentWayPointIndex];

        transform.position = Vector3.MoveTowards(transform.position, Managers.Game.wayPoints[realWayPointIndex].transform.position,
            Time.deltaTime * enemyData.MoveSpeed);

        Vector3 dir = Managers.Game.wayPoints[realWayPointIndex].transform.position - transform.position;

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
        if (Vector2.Distance(Managers.Game.wayPoints[realWayPointIndex].transform.position, transform.position) < 0.01f)
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

    public void InitObjectData()
    {
        throw new System.NotImplementedException();
    }
}