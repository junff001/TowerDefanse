using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : BuffBase
{
    EnemyBase enemy;
    private float damageTime;
    private static readonly float dotTime = 1;

    public Dot(GameObject affecter, float duration, float amplification)
    {
        buffType = Define.BuffType.DEBUFF;
        base.amplification = amplification;
        base.duration = duration;
        base.affecter = affecter;
    }

    public override void Update()
    {
        base.Update();
        damageTime += Time.deltaTime;

        if(damageTime >= dotTime)
        {
            damageTime -= dotTime;
            HealthSystem health = enemy.GetComponent<HealthSystem>();
            health.TakeDamage(amplification, propertyType);
        }
    }

    public override void Destroy()
    {
        base.Destroy();
    }
}