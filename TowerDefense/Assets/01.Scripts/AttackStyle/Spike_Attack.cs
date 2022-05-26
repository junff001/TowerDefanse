using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Attack : CoreBase
{
    [SerializeField] private ParticleSystem hitEffect = null;
    [SerializeField] private Sprite onSpike = null;
    [SerializeField] private Sprite offSpike = null;

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);

        hitEffect.transform.position = -enemy.transform.up;
    }
}
