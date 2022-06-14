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

    public virtual void OnEnable()
    {
        StartCoroutine(OnRader());
        StartCoroutine(OnAttack());

        PropertyCheck();
    }

    public void SetTarget(LayerMask priorityMask = default) // 우선순위나 이동 거리에 따라서 타겟 설정
    {
        if (enemies.Count <= 0) return;

        enemies.Sort((x, y) => x.movedDistance.CompareTo(y.movedDistance)); 
           
        if(false == priorityMask.Equals(default)) // 우선 타겟팅할 적이 있다면
        {
            target = enemies.Find(x => x.gameObject.layer == priorityMask);
        }

        if(target == null)
            target = enemies[0];
    }

    public virtual void OnDisable()
    {
        StopCoroutine(OnRader());
        StopCoroutine(OnAttack());
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    public virtual IEnumerator OnRader()
    {
        while (true)
        {
            EnemyRader(enemyMask);
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
        SetTarget();
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
    public virtual IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies.Count > 0);
            
            if(target != null)
                Attack(towerData.OffensePower, target.healthSystem);

            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
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
