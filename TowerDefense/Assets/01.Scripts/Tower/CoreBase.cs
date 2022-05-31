using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class CoreBase : MonoBehaviour
{
    [SerializeField] private float gizmoHeight = 0f;

    public LayerMask enemyMask = default;                      // 적을 분별하는 마스크
    protected Collider2D[] enemies = null;                         // 공격 범위 안에 있는 적들
    protected Collider2D currentTarget { get; set; } = null;       // 현재 타겟

    public TowerData towerData { get; set; } = default;
    public eCoreName coreType;

    public virtual void Start()
    {
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    protected IEnumerator Rader()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            enemies = EnemyRader(enemyMask); 
        }
    }

    // 공격 범위 처리 함수
    Collider2D[] EnemyRader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position + new Vector3(0, gizmoHeight, 0), towerData.AttackRange, targetMask);
    }

    // 공격 범위 기즈모 표시
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, gizmoHeight, 0), towerData.AttackRange);
            Gizmos.color = Color.white;
        }
    }
#endif

    // 공격 실행 함수
    public virtual IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            Collider2D guardian = enemies.ToList().Find((x => x.gameObject.layer == 1 << LayerMask.NameToLayer("GuardianEnemy"))); // Guardian

            if(guardian == null)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (i >= towerData.attackTargetCount)
                        break;

                    if (enemies[i] != null)
                    {
                        Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                    }
                }
            }
            else
            {
                Attack(towerData.OffensePower * enemies.Length, guardian.GetComponent<HealthSystem>());
            }
            
        
            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 怨듭냽留뚰겮 湲곕떎由ш퀬,
        }
    }

    // 공격 로직 함수
    public abstract void Attack(int power, HealthSystem enemy);
}
