using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public HealthSystem healthSystem;
    public EnemyData enemyData = new EnemyData();
    public EnemyAttackData enemyAttackData = new EnemyAttackData();

    [HideInInspector] public SpineController spineController;

    List<BuffBase> buffList = new List<BuffBase>();
    MeshRenderer mesh = null;
    SpriteRenderer spriteRenderer;
    Transform target;
    ContactFilter2D contactFilter = new ContactFilter2D();

    [SerializeField] ParticleSystem suicideBombingEffect;
    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    public int CurrentWayPointIndex => currentWayPointIndex;
    private int realWayPointIndex = 0;

    public float aliveTime { get; set; }
    public float movedDistance { get; set; }

    public bool IsDead => healthSystem.IsDead();
    bool canSuicideBombing = false;
    bool canThrowing = true;

    CircleCollider2D atkRangeCollider;

    protected virtual void Awake()
    {
        healthSystem = GetComponent<HealthSystem>();
        mesh = GetComponent<MeshRenderer>();
        spineController = GetComponent<SpineController>();
        atkRangeCollider = GetComponent<CircleCollider2D>();
    }

    protected virtual void Start()
    {
        mesh.sortingLayerName = "Character";
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)enemyData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)enemyData.Shield, true);
        healthSystem.OnDied += () =>
        {
            Managers.Gold.GoldPlus(enemyData.RewardGold);
            spineController.Die();
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

        if (enemyData.IsThrower)
        {
            float originMoveSpeed = enemyData.MoveSpeed;

            if (TargetInATKRange() != null)
            {
                target = TargetInATKRange().transform;
            }
            
            if (target != null)
            {
                enemyData.MoveSpeed = 0f;
                if (canThrowing)
                {
                    ThrowProjectile();
                    StartCoroutine(ThrowDelay());
                }
                
            }

            enemyData.MoveSpeed = originMoveSpeed;
        }

        if (enemyData.IsSuicideBomber)
        {
            float originMoveSpeed = enemyData.MoveSpeed;

            if (TargetInATKRange() != null)
            {
                target = TargetInATKRange().transform;
            }

            if (target != null)
            {
                TargetChase();

                if (IsCollisionTarget())
                {
                    SuicideBombing();
                }
            }
        }
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

    public void InitAnimController()
    {
        this.gameObject.layer = LayerMask.NameToLayer(enemyData.MonsterType.ToString() + "Enemy");
        if (enemyData.IsHide) // 은신 체크
        {
            spineController.skeleton.A = 0.5f;
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

    void ThrowProjectile()
    {
        var projectile = Managers.Pool.GetItem<Bomb>();
        projectile.transform.position = transform.position;
        projectile.InitProjectileData(enemyData.OffensePower, target, null);
    }

    IEnumerator ThrowDelay()
    {
        canThrowing = false;
        yield return new WaitForSeconds(enemyAttackData.attackSpeed);
        canThrowing = true;
    }

    Collider2D TargetInATKRange()
    {
        return Physics2D.OverlapCircle(transform.position, enemyAttackData.atkRangeRadius, enemyAttackData.opponentLayer);
    }

    bool IsCollisionTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.5f)
        {
            Debug.Log("도착함");
            return true;
        }
        else
        {
            return false;
        }
    }

    void SuicideBombing()
    {
        if (enemyData.HP <= 0)
        {
            return;
        }

        var effect = Instantiate(suicideBombingEffect);
        effect.transform.position = transform.position;
        effect.Play();
        enemyData.HP = 0f;

        Debug.Log("자폭 가동");
    }

    void TargetChase()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime);
    }
}