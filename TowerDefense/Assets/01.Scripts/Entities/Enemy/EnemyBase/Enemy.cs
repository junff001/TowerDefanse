using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : LivingEntity
{
    [HideInInspector] public HealthSystem healthSystem;
    [HideInInspector] public SpineController spineController;
    [HideInInspector] public Transform target;

    public EnemyData enemyData;
    public EnemyAttackData enemyAttackData = new EnemyAttackData();
    [SerializeField] ParticleSystem suicideBombingEffect;

    List<BuffBase> buffList = new List<BuffBase>();
    MeshRenderer mesh = null;
    public BoxCollider2D myCollider;

    public int wayPointListIndex { get; set; }
    private int currentWayPointIndex = 0;
    public int CurrentWayPointIndex => currentWayPointIndex;
    private int realWayPointIndex = 0;

    public float aliveTime { get; set; }
    public float movedDistance { get; set; }

    public bool IsDead => healthSystem.IsDead();
    bool canThrowing = true;

    protected virtual void Awake()
    {    
        livingEntityData = new EnemyData();
        enemyData = livingEntityData as EnemyData;

        myCollider         = GetComponent<BoxCollider2D>();
        healthSystem       = GetComponent<HealthSystem>();
        mesh               = GetComponent<MeshRenderer>();
        spineController    = GetComponent<SpineController>();

        StartCoroutine(CheckTile());
    }

    protected virtual void Start()
    {
        mesh.sortingLayerName = "Character";
        healthSystem.SetAmountMax(eHealthType.HEALTH, (int)enemyData.HP, true);
        healthSystem.SetAmountMax(eHealthType.SHIELD, (int)enemyData.ShieldAmount, true);
        healthSystem.OnDied += () =>
        {
            //Debug.Log("죽음");
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
        CheckBuffs();

        ThrowBomb(); // 폭탄 던지는 얘면 던지고~

        if(IsMovingToTarget() == false) // 자폭병이면 자폭할 대상과 가까운지 체크, 가까우면 자폭하러 이동, 아니면 그냥 무브!
        {
            Move();
        }
    }

    public void ThrowBomb()
    {
        if (enemyData.IsThrower)
        {
            Collider2D attackbleTarget = TargetInATKRange();
            target = attackbleTarget != null ? attackbleTarget.transform : null;

            if (target != null)
            {
                if (canThrowing)
                {
                    ThrowProjectile();
                    StartCoroutine(ThrowDelay());
                }
            }
        }
    }

    public bool IsTargetInMySuicideRange()
    {
        if(target.transform.position.y >= transform.position.y) // 내 위쪽이나 같으면,
        {
            return Vector2.Distance(target.transform.position, transform.position) <= enemyAttackData.suicideRange;
        }
        else
        {
            // 무조건 아래로 0.5 내려서 타워를 설치함 -> 동일한 거리 차이로 설치하려 해도, 플레이어 기준으로 1의 거리차이가 나버림.
            return Vector2.Distance(target.transform.position, transform.position) <= enemyAttackData.suicideRange + 1; 
        }
    }

    public bool IsMovingToTarget()
    {
        if (enemyData.IsThrower == false && Managers.Wave.GameMode == Define.GameMode.OFFENSE && IsTargetInMySuicideRange())
        {
            TargetChase();
            if (IsCollisionTarget()) SuicideBombing();

            return true; // 만약 타겟 찾아서 쫓아가면 무브는 하면 안되니까..
        }
        return false;
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
        projectile.InitProjectileData(target, null, enemyAttackData.opponentLayer);
    }

    IEnumerator ThrowDelay()
    {
        canThrowing = false;
        yield return new WaitForSeconds(enemyAttackData.attackSpeed);
        canThrowing = true;
    }

    Collider2D TargetInATKRange()
    {
        return Physics2D.OverlapCircle(transform.position, enemyAttackData.atkRange, enemyAttackData.opponentLayer);
    }

    bool IsCollisionTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.5f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void SuicideBombing()
    {
        if (IsDead) return;
       
        target.GetComponent<HealthSystem>().TakeDamage(enemyAttackData.explosionDamage);
        healthSystem.TakeDamage(healthSystem.GetAmount(eHealthType.HEALTH), true); // 혹시 실드 있어도 그냥 터져야 하니까 관통!
        
        var effect = Instantiate(suicideBombingEffect);
        effect.transform.position = transform.position;
        effect.Play();
    }

    void TargetChase()
    {
        transform.position = Vector2.MoveTowards(transform.position, target.position, Time.deltaTime * enemyData.MoveSpeed * 3f);
    }


    IEnumerator CheckTile()
    {
        while(true)
        {
            myCollider.enabled = !Managers.Build.IsInTunnel(transform.position);
            yield return new WaitForSeconds(0.1f);
        }
    }
}