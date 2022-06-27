using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class CoreBase : MonoBehaviour
{
    [Header("레이더")]
    [SerializeField] protected float raderHeight = 0f;

    private LayerMask enemyMask = default;                           // 적을 분별하는 마스크    
    protected List<EnemyBase> enemies = new List<EnemyBase>();      // 공격 범위 안에 있는 적들
    protected EnemyBase target { get; set; } = null;                // 현재 타겟

    public TowerData TowerData { get; set; } = default;
    public eCoreName coreType;
    public BuffBase Buff { get; set; } 

    protected Bullet bullet = null;

    //[SerializeField] private MonsterType priorityTargetType;
    [SerializeField] private MonsterType attackableTargetType;

    public virtual void OnEnable()
    {
        BuffAaccordingToProperty();
        StartCoroutine(OnRader());
        StartCoroutine(CoAttack());
    }

    private void Start()
    {
        enemyMask = LayerMask.GetMask("Enemy");
    }

    public bool IsTargetOutOfRange() // 때리던 애가 내 범위에서 벗어나면.
    {
        if (target == null) return false; // 그냥 넘어가요~

        return Vector2.Distance(target.transform.position, transform.position) >= TowerData.AttackRange;
    }

    public void SetTarget() // 우선순위나 이동 거리에 따라서 타겟 설정
    {
        if (target != null && target.IsDead) target = null;
        if (enemies.Count <= 0) return;

        for(int i = 0; i< enemies.Count; i++)
        {
            if((enemies[i].enemyData.MonsterType & attackableTargetType) == 0)
            {
                enemies.RemoveAt(i);
                i--;
            }
        }

        enemies.Sort((x, y) => x.movedDistance.CompareTo(y.movedDistance)); // 맨 앞 놈 패야 하니까.
        target = enemies[0];

        /*
        우선 타게팅이 나중에 필요할지도 모르자나
        EnemyBase priorityTarget = enemies.Find(x => (x.enemyData.MonsterType & priorityTargetType) != 0);
        
        if(priorityTarget != null)
        {
            target = priorityTarget;
        }
        
        if(target == null)
        {
            target = enemies[0];
        }
        */
    }

    public virtual void OnDisable()
    {
        StopCoroutine(OnRader());
        StopCoroutine(CoAttack());
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    public virtual IEnumerator OnRader()
    {
        while (true)
        {
            EnemyRader();
            SetTarget();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 범위 처리 함수
    public void EnemyRader()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position - new Vector3(0, raderHeight, 0), TowerData.AttackRange, enemyMask);
        enemies.Clear();
        for(int i =0; i< cols.Length; i++)
        {
            enemies.Add(cols[i].GetComponent<EnemyBase>());
        }
    }

    // 공격 범위 기즈모 표시
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, raderHeight, 0), TowerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif

    // 공격 실행 함수
    public virtual IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => target != null && target.IsDead == false);
            Attack(TowerData.AttackPower, target.healthSystem);
           
            yield return new WaitForSeconds(1f / TowerData.AttackSpeed);
        }
    }

    public virtual void OnAttack()
    {
        bullet.transform.SetParent(Managers.Pool.poolInitPos);
        bullet = null;
        if (IsTargetOutOfRange()) target = null;
    }

    // 공격 로직 함수
    public abstract void Attack(int power, HealthSystem enemy);

    void BuffAaccordingToProperty()
    {
        switch (TowerData.Property)
        {
            case PropertyType.NONE: Buff = null; break;
            case PropertyType.FIRE: Buff = new Dot(target.gameObject, 3f, 20f); break;
            case PropertyType.WATER: Buff = new Slow(target.gameObject, 3f, 50f); break;
            case PropertyType.SOIL: Buff = new Splash(3f, 10f, Resources.Load<ParticleSystem>("JMO Assets/Cartoon FX/CFX Prefabs/Hits/CFX_Hit_A Red+RandomText"), enemyMask, TowerData.Property); break;
            case PropertyType.LIGHTNING: Buff = new Chain(target.gameObject, 30f); break;
            case PropertyType.GRASS: Buff = new Restriction(target.gameObject, 3f); break;
        }
    }
}
