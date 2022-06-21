using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Restriction : BuffBase
{
    EnemyBase enemy;
    float natureMoveSpeed;

    public Restriction(GameObject affecter, float duration)
    {
        buffType = Define.BuffType.DEBUFF;
        base.duration = duration;
        base.affecter = affecter;
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
