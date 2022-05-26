using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Attack : CoreBase
{
    [SerializeField] private SpriteRenderer spriteObj = null;
    [SerializeField] private ParticleSystem hitEffect = null;
    [SerializeField] private Sprite onSpike = null;
    [SerializeField] private Sprite offSpike = null;
    [SerializeField] private float boundaryRadius = 0f;

    public override void Start()
    {
        //StartCoroutine(BoundaryRader());
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    } 

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);
        hitEffect.transform.position = -enemy.transform.up;
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            for (int i = 0; i < enemies.Length; i++)
            {
                if (i >= towerData.attackTargetCount)
                    break;

                if (enemies[i] != null)
                {
                    spriteObj.sprite = onSpike;
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고
            spriteObj.sprite = offSpike;
            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고
        }
    }

    IEnumerator BoundaryRader()
    {
        while (true)
        {
            if (BoundaryLine())
            {
                spriteObj.sprite = onSpike; 
            }
            else if (BoundaryLine() == null)
            {
                spriteObj.sprite = offSpike;
            }

            yield return new WaitForSeconds(0.1f);
        }
    }

    // 공격 태세에 들어가는 경계선
    Collider2D BoundaryLine()
    {
        return Physics2D.OverlapCircle(transform.position + new Vector3(0, gizmoHeight, 0), boundaryRadius, LayerMask.GetMask("Enemy"));
    }

    // 경계선 기즈모 표시
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position + new Vector3(0, gizmoHeight, 0), boundaryRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
