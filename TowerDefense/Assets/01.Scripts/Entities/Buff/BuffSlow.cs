using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSlow : BuffBase
{
    public float changedValue;

    public BuffSlow(GameObject _affecter, float _duration, float _amplification) : base(_affecter, _duration, _amplification)
    {
        buffType = Define.BuffType.DEBUFF;
    }

    public override void Init()
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
        base.Destroy();

        EnemyBase enemy = affecter.GetComponent<EnemyBase>();
        enemy.enemyData.MoveSpeed += changedValue;
    }
}