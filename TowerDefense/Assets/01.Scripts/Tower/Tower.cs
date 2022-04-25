using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float attackRange = 0f;
    private int cardLevel = 0;
    private float raderRadius = 0f;
    private float offensePower = 0f;
    private float attackSpeed = 1f;
    private int attackTargetCount = 1;
    private int blockTargetCount = 1;

    private LayerMask enemyMask = default;
    private Collider2D[] enemies = null;

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

    // 공격 범위 처리
    Collider2D[] EnemyRader(LayerMask targetMask)
    {
        return Physics2D.OverlapCircleAll(transform.position, attackRange, targetMask);
    }

    // 공격 처리 
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
                if (i >= attackTargetCount)
                    break; // 공격 가능 대상 수만큼 때렸으면 그만 때리기
                if (enemies[i] != null)
                {
                    Attack(offensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / attackSpeed); // 공속만큼 기다리고,
        }
    }

    // 공격 처리
    public virtual void Attack(float power, HealthSystem enemy)
    {
        var bullet = PoolManager.GetObject();
        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }

    // 공격 범위 기즈모 표시
#if UNITY_EDITOR 
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
            Gizmos.color = Color.white;
        }
    }
#endif
}
