using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float HP;
    public float Shield;
    public int OffensePower;
    public float MoveSpeed;
    public int RewardGold;
    public Define.MonsterType MonsterType;
    public Define.SpeciesType SpeciesType;

    public bool IsHide     = false;
    public bool IsShilde   = false;
    public bool IsArmor      = false;
    public bool IsWitch      = false;
    public bool IsAlchemist  = false;
    public bool IsFly        = false;
}

public abstract class EnemyBase : MonoBehaviour
{
    public Vector3 targetPos = Vector3.zero; // 이거는 나중에 매직같은거 할 때
    [HideInInspector] public HealthSystem healthSystem;
    [HideInInspector] public EnemyData enemyData = new EnemyData();
    [HideInInspector] public SpineController sc;

    private List<BuffBase> buffList = new List<BuffBase>();
    private MeshRenderer mesh = null;

    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    public int CurrentWayPointIndex { get => currentWayPointIndex; private set { } }
    private int realWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

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

    public void InitEnemyData(EnemySO enemySO)
    {
        enemyData.HP            = enemySO.HP;
        enemyData.Shield        = enemySO.Shield;
        enemyData.OffensePower  = enemySO.OffensePower;
        enemyData.MoveSpeed     = enemySO.MoveSpeed;
        enemyData.RewardGold    = enemySO.RewardGold;
        enemyData.MonsterType   = enemySO.MonsterType;
        enemyData.SpeciesType   = enemySO.SpeciesType;
        enemyData.IsHide        = enemyData.MonsterType.HasFlag(Define.MonsterType.Hide);
        enemyData.IsShilde      = enemyData.MonsterType.HasFlag(Define.MonsterType.Shield);
        enemyData.IsArmor       = enemyData.MonsterType.HasFlag(Define.MonsterType.Armor);
        enemyData.IsFly         = enemyData.MonsterType.HasFlag(Define.MonsterType.Fly);
    }

    public void InitAnimController()
    {
        this.gameObject.layer = LayerMask.NameToLayer(enemyData.MonsterType.ToString() + "Enemy");
        if (enemyData.IsHide) // 은신 체크
        {
            sc.Skeleton.A = 0.5f;
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
}