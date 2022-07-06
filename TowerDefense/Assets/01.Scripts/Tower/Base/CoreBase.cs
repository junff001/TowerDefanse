using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public abstract class CoreBase : MonoBehaviour
{
    [Header("레이더")]
    [SerializeField] protected float raderHeight = 0f;

    protected LayerMask opponentLayer = default;                           // 적을 분별하는 마스크    
    protected List<Enemy> enemies = new List<Enemy>();      // 공격 범위 안에 있는 적들
    protected Enemy target { get; set; } = null;                // 현재 타겟

    public TowerData towerData = new TowerData(); 
    public eCoreName coreType;
    public BuffBase Buff { get; set; } 

    protected Projectile bullet = null;

    [SerializeField] private MonsterType notAttackableType;

    public virtual void OnEnable()
    {
        StartCoroutine(EnemyRader());
        StartCoroutine(AttackDelay());
    }

    private void Start()
    {
        opponentLayer = LayerMask.GetMask("Enemy");
    }

    public bool IsTargetOutOfRange() // 때리던 애가 내 범위에서 벗어나면.
    {
        if (target == null) return false; // 그냥 넘어가요~

        return Vector2.Distance(target.transform.position, transform.position) >= towerData.AttackRange;
    }

    public void SetTarget() // 우선순위나 이동 거리에 따라서 타겟 설정
    {
        if (target != null && target.IsDead) target = null;
        if (enemies.Count <= 0) return;


        enemies.Sort((x, y) => -x.movedDistance.CompareTo(y.movedDistance));

        for (int i = 0; i< enemies.Count; i++)
        {
            if((enemies[i].enemyData.MonsterType & notAttackableType) == 0) // 공격 불가한 타입이 없는 친구면!
            {
                target = enemies[0];
                break;
            }
        }
    }

    public virtual void OnDisable()
    {
        StopCoroutine(EnemyRader());
        StopCoroutine(AttackDelay());
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    public virtual IEnumerator EnemyRader()
    {
        while (true)
        {
            SetEnemies();
            SetTarget();
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 범위 처리 함수
    public void SetEnemies()
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll
            (transform.position - new Vector3(0, raderHeight, 0), towerData.AttackRange, opponentLayer);
        enemies.Clear();
        for(int i =0; i< cols.Length; i++)
        {
            enemies.Add(cols[i].GetComponent<Enemy>());
        }
    }

    // 공격 실행 함수
    public virtual IEnumerator AttackDelay()
    {
        while (true)
        {
            yield return new WaitUntil(() => target != null && target.IsDead == false);
            if(target != null)
            {
                Attack(target.healthSystem, opponentLayer);
            }
            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
    }

    public virtual void OnAttack()
    {
        bullet.transform.SetParent(Managers.Pool.poolInitPos);
        bullet = null;
        if (IsTargetOutOfRange()) target = null;
    }

    // 공격 로직 함수
    public abstract void Attack(HealthSystem enemy, LayerMask opponentLayer);

}
