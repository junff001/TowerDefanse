using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : Bullet
{
    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData,enemyTrm);
        EnemyBase targetEnemy = enemyTrm.GetComponent<EnemyBase>();
        targetPos = GetExpectPos(targetEnemy, maxTime);
    }

    public override void Shoot()
    {
        
    }
}
