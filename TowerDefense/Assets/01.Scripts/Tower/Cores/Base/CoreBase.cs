using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreBase : MonoBehaviour
{
    [Header("레이더")]
    [SerializeField] private float raderHeight = 0f;

    public LayerMask enemyMask = default;                          // 적을 분별하는 마스크    
    protected Collider2D[] enemies = null;                         // 공격 범위 안에 있는 적들
    protected Collider2D currentTarget { get; set; } = null;       // 현재 타겟

    public TowerData towerData { get; set; } = default;
    public eCoreName coreType;

    public virtual void OnEnable()
    {
        StartCoroutine(OnRader());
        StartCoroutine(OnAttack());

        PropertyCheck();
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
            enemies = Rader(enemyMask);
            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 범위 처리 함수
    public virtual Collider2D[] Rader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position + new Vector3(0, raderHeight, 0), towerData.AttackRange, targetMask);
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
            yield return new WaitUntil(() => enemies.Length > 0);
            currentTarget = enemies[0];
            Attack(towerData.OffensePower, currentTarget.GetComponent<HealthSystem>());

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
