using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CoreBase : MonoBehaviour
{
    private LayerMask enemyMask = default;                      // 적을 분별하는 마스크
    private Collider2D[] enemies = null;                        // 공격 범위이 안에 있는 적들
    public Collider2D currentTarget { get; set; } = null;       // 현재 타겟

    public TowerData towerData;
    public CoreType coreType;

    void Start()
    {
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    // 0.1초 텀을 두고 공격 범위 체크 처리
    IEnumerator Rader()
    {
        enemyMask = LayerMask.GetMask("Enemy");
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            enemies = EnemyRader(enemyMask);
        }
    }

    // 공격 범위 처리 함수
    Collider2D[] EnemyRader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position, towerData.AttackRange, targetMask);
    }

    // 공격 실행 함수
    IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies != null && enemies.Length > 0);

            List<EnemyBase> enemyList = new List<EnemyBase>();
            for (int i = 0; i < enemies.Length; i++)
            {
                if (enemies[i] != null)
                    enemyList.Add(enemies[i].GetComponent<EnemyBase>());
            }

            enemyList.Sort((x, y) => x.movedDistance.CompareTo(y.movedDistance));

            for (int i = 0; i < enemies?.Length; i++) // 공격 
            {
                if (i >= towerData.attackTargetCount)
                    break; // 공격 가능 대상 수만큼 때렸으면 그만 때리기
                if (enemies[i] != null)
                {
                    currentTarget = enemies[i];
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고,
        }
    }

    // 공격 로직 함수
    public abstract void Attack(int power, HealthSystem enemy);
}
