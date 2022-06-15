using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slow : BuffBase
{
    public float changedValue;

    public Slow(GameObject _affecter, float _duration, float _amplification) : base(_affecter, _duration, _amplification)
    {
        buffType = Define.BuffType.DEBUFF;
    }

    public override void Initialization()
    {
        EnemyBase enemy = affecter.GetComponent<EnemyBase>();

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
        EnemyBase enemy = affecter.GetComponent<EnemyBase>();
        enemy.enemyData.MoveSpeed += changedValue;

        base.Destroy();
    }
}