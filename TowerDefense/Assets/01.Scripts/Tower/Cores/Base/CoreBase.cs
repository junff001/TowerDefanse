using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CoreBase : MonoBehaviour
{
    [Header("레이더")]
    [SerializeField] protected float raderHeight = 0f;

    public LayerMask enemyMask = default;                          // 적을 분별하는 마스크    
    protected List<EnemyBase> enemies = new List<EnemyBase>();                         // 공격 범위 안에 있는 적들
    protected EnemyBase target { get; set; } = null;       // 현재 타겟

    public TowerData towerData { get; set; } = default;
    public eCoreName coreType;

    protected Bullet bullet = null;

    public virtual void OnEnable()
    {
        StartCoroutine(OnRader());
        StartCoroutine(CoAttack());

        PropertyCheck();
    }

    public bool IsTargetOutOfRange() // 때리던 애가 내 범위에서 벗어나면.
    {
        if (target == null) return false; // 그냥 넘어가요~
        return Vector2.Distance(target.transform.position, transform.position) > towerData.AttackRange + 1;
    }

    public void SetTarget(LayerMask priorityMask = default) // 우선순위나 이동 거리에 따라서 타겟 설정
    {
        if (enemies.Count <= 0)
        {
            return;
        }
        enemies.Sort((x, y) => x.movedDistance.CompareTo(y.movedDistance)); // 맨 앞 놈 패야 하니까.

        if (false == priorityMask.Equals(default)) // 우선 타겟팅할 적이 있다면
        {
            EnemyBase priorityTarget = enemies.Find(x => x.gameObject.layer == priorityMask);
            
            if(priorityTarget != null)
            {
                target = priorityTarget;
            }
        }
        
        if(target == null)
        {
            target = enemies[0];
        }
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
            EnemyRader(enemyMask);
            SetTarget(enemyMask);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 범위 처리 함수
    public void EnemyRader(LayerMask targetMask)
    {
        Collider2D[] cols = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, raderHeight, 0), towerData.AttackRange, targetMask);
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
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, raderHeight, 0), towerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif

    // 공격 실행 함수
    public virtual IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => target != null);
            Attack(towerData.OffensePower, target.healthSystem);
            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
    }

    public virtual void OnAttack()
    {
        bullet = null;
        if (IsTargetOutOfRange()) target = null;
    }

    // 공격 로직 함수
    public abstract void Attack(int power, HealthSystem enemy);

    public virtual void PropertyCheck()
    {
        // 애니메이션 이나 그런거 넣으셈
        switch (towerData.Property)
        {
            case Define.PropertyType.WATER:
                break;
            case Define.PropertyType.FIRE:
                break;
            case Define.PropertyType.LIGHTNING:
                break;
            case Define.PropertyType.LIGHT:
                break;
            case Define.PropertyType.DARKNESS:
                break;
        }
    }
}
