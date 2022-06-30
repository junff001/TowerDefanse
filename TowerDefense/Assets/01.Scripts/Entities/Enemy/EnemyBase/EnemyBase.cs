using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour
{
    public HealthSystem healthSystem;
    public EnemyData enemyData = new EnemyData();
    [HideInInspector] public SpineController sc;

    List<BuffBase> buffList = new List<BuffBase>();
    MeshRenderer mesh = null;
    SpriteRenderer spriteRenderer;
    Transform target;
    List<Collider2D> opponentColliders = new List<Collider2D>();
    ContactFilter2D contactFilter = new ContactFilter2D();

    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    public int CurrentWayPointIndex => currentWayPointIndex;
    private int realWayPointIndex = 0;

    public float aliveTime { get; set; }
    public float movedDistance { get; set; }

    public bool IsDead => healthSystem.IsDead();
    bool canSuicideBombing = false;

    [SerializeField] int blinkingCount;
    [SerializeField] float blinkingDelay;
    [SerializeField] float explosionDamage;
    [SerializeField] float atkRangeRadius;
    [SerializeField] float attackSpeed;
    [SerializeField] LayerMask opponentLayer;

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

        if (enemyData.IsThrower)
        {
            float originMoveSpeed = enemyData.MoveSpeed;

            if (IsRangeInTarget() > 0)
            {
                target = opponentColliders[0].transform;
                enemyData.MoveSpeed = 0f;
                ThrowProjectile();
                StartCoroutine(ThrowDelay());
            }

            enemyData.MoveSpeed = originMoveSpeed;
        }

        if (enemyData.IsSuicideBomber)
        {
            float originMoveSpeed = enemyData.MoveSpeed;

            if (IsRangeInTarget() > 0)
            {
                target = opponentColliders[0].transform;
                enemyData.MoveSpeed = 0f;
                StartCoroutine(TargetDiscoverySiren());

                if (canSuicideBombing)
                {
                    enemyData.MoveSpeed = originMoveSpeed;
                    TargetChase();

                    if (IsCollisionTarget())
                    {
                        SuicideBombing();
                    }
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

    void ThrowProjectile()
    {
        var projectile = Managers.Pool.GetItem<Bomb>();
        projectile.InitProjectileData(enemyData.OffensePower, target, null);
    }

    IEnumerator ThrowDelay()
    {
        yield return new WaitForSeconds(attackSpeed);
    }

    int IsRangeInTarget()
    {
        contactFilter.SetLayerMask(opponentLayer);
        return Physics2D.OverlapCircle(transform.position, atkRangeRadius, contactFilter, opponentColliders);
    }

    bool IsCollisionTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator TargetDiscoverySiren()
    {
        int originCount = blinkingCount;

        while (blinkingCount <= 0)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkingDelay);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkingDelay);
            blinkingCount--;
        }


        blinkingCount = originCount;
        canSuicideBombing = true;
    }

    void SuicideBombing()
    {
        target.GetComponent<HealthSystem>().TakeDamage(explosionDamage);
        Destroy(this);
    }

    void TargetChase()
    {
        target = opponentColliders[0].transform;
        transform.Translate(target.position * enemyData.MoveSpeed * Time.deltaTime);
    }
}