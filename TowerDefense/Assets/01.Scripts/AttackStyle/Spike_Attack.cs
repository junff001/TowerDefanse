using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Attack : CoreBase
{
    [SerializeField] private ParticleSystem hitEffect = null;

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);

        hitEffect.transform.position = -enemy.transform.up;
    }
}
