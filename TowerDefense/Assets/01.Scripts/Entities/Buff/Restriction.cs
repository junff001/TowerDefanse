using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restriction : BuffBase
{
    EnemyBase enemy;
    float natureMoveSpeed;

    public Restriction(GameObject affecter, float duration, float amplification) : base(affecter, duration, amplification)
    {
        buffType = Define.BuffType.DEBUFF;
    }

    public override void Initialization()
    {
        enemy = affecter.GetComponent<EnemyBase>();

        if (enemy != null)
        {
            natureMoveSpeed = enemy.enemyData.MoveSpeed;
            enemy.enemyData.MoveSpeed = 0;
        }
    }

    public override void Destroy()
    {
        enemy.enemyData.MoveSpeed = natureMoveSpeed;
        base.Destroy();
    }
}
