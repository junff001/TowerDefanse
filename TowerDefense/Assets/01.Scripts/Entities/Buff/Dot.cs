using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : BuffBase
{
    EnemyBase enemy;
    private float damageTime;
    private static readonly float dotTime = 1;

    public Dot(GameObject _affecter, float _duration, float _amplification) : base(_affecter, _duration, _amplification)
    {
        buffType = Define.BuffType.DEBUFF;
    }

    public override void Initialization()
    {
        enemy = affecter.GetComponent<EnemyBase>();
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