using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : BuffBase
{
    public float changedValue;

    public Slow(GameObject affecter, float duration, float amplification)
    {
        buffType = Define.BuffType.DEBUFF;
        base.affecter = affecter;
        base.duration = duration;
        base.amplification = amplification;

        Enemy enemy = affecter.GetComponent<Enemy>();

        if (enemy != null)
        {
            changedValue = enemy.enemyData.MoveSpeed * (amplification / 100);
            enemy.enemyData.MoveSpeed -= changedValue;
        }
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Destroy()
    {
        Enemy enemy = affecter.GetComponent<Enemy>();
        enemy.enemyData.MoveSpeed += changedValue;

        base.Destroy();
    }
}