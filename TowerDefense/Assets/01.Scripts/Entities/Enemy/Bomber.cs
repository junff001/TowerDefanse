using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class Bomber : EnemyBase
{
    [SerializeField] float atkRangeRadius;
    [SerializeField] float attackSpeed;
    [SerializeField] LayerMask opponentLayer;

    float originMoveSpeed;
    List<Collider2D> opponentColliders = new List<Collider2D>();
    ContactFilter2D contactFilter = new ContactFilter2D();

    protected override void Update()
    {
        base.Update();  

        if (IsRangeInTarget() > 0)
        {
            originMoveSpeed = enemyData.MoveSpeed;
            enemyData.MoveSpeed = 0;

            ThrowProjectile(opponentColliders[0].transform);
            StartCoroutine(ThrowDelay(attackSpeed));
        }
        else
        {
            enemyData.MoveSpeed = originMoveSpeed;
        }
    }

    void ThrowProjectile(Transform target)
    {
        var projectile = Managers.Pool.GetItem<Bomb>();
        projectile.InitProjectileData(enemyData.OffensePower, target, null);
    }

    IEnumerator ThrowDelay(float attackSpeed)
    {
        yield return new WaitForSeconds(attackSpeed);   
    }
    
    int IsRangeInTarget()
    {
        contactFilter.SetLayerMask(opponentLayer);
        return Physics2D.OverlapCircle(transform.position, atkRangeRadius, contactFilter, opponentColliders);
    }
}
