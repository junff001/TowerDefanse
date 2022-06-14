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
    public bool IsHide;
    public bool IsGuardian;
    public bool IsFlying;
    public bool IsDebuffIimmune;
    public Define.PropertyType PropertyResistance;
    public Define.MonsterType MonsterType;
}

public abstract class EnemyBase : MonoBehaviour
{
    public Vector3 targetPos = Vector3.zero; // 이거는 나중에 매직같은거 할 때

    [HideInInspector] public HealthSystem healthSystem;

    [SerializeField] protected EnemySO enemySO;
    [HideInInspector] public EnemyData enemyData = new EnemyData();

    private List<BuffBase> buffList = new List<BuffBase>();
    private MeshRenderer mesh = null;

    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    private int realWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    SpineController animController;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        mesh = GetComponent<MeshRenderer>();
        animController = GetComponent<SpineController>();
    }

    protected virtual void Start()
    {
        mesh.sortingLayerName = "Character";
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)enemyData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)enemyData.Shield, true);
        healthSystem.OnDied += () =>
        {
            Managers.Gold.GoldPlus(enemyData.RewardGold);
            animController.Die();
            Managers.Wave.aliveEnemies.Remove(this);
            Managers.Wave.CheckWaveEnd();
            transform.GetChild(0).gameObject.SetActive(false);

            GetComponent<Collider2D>().enabled = false;
            healthSystem.enabled = false;          // 스크립트는 종료, 나중에 풀링할거면 이거 켜는 함수 만들어줘
            enabled = false;                       // ''
            Invoke("Die", 3f); // 선한쌤이 핏자국 남기라고 하셨던 거 같음 시체나..
        };
    }

    public void MakeEffectObj()
    {
        GameObject makeObj = null;
        switch (enemySO.PropertyResistance)
        {
            case Define.PropertyType.DARKNESS: makeObj   = Managers.Wave.darknessAura;  break;
            case Define.PropertyType.LIGHT: makeObj      = Managers.Wave.lightAura;     break;
            case Define.PropertyType.LIGHTNING: makeObj  = Managers.Wave.lightningAura; break;
            case Define.PropertyType.WATER: makeObj      = Managers.Wave.waterAura;     break;
            case Define.PropertyType.FIRE: makeObj       = Managers.Wave.fireAura;      break;
        }

        if (makeObj != null)
        {
            Instantiate(makeObj, this.transform);
        }
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
        if (buff.buffType == Define.BuffType.DEBUFF && enemyData.IsDebuffIimmune) return;

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

    public void SetStartWaypoint() // 고블린 통 같은 것으로 고블린을 소환했을 때, 웨이포인트를 어떻게 설정해줄지
    {
        //List<Transform> wayPoints = Managers.Game.wayPoints.ToList();
        //
        //wayPoints.Sort((x,y) => x.position.x > );
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