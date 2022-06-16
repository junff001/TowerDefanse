using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Spike_Core : CoreBase
{
    [SerializeField] private SpriteRenderer spriteObj = null;
    [SerializeField] private ParticleSystem hitEffect = null;
    [SerializeField] private Sprite onSpike = null;
    [SerializeField] private Sprite offSpike = null;

    public override void OnEnable()
    {
        StartCoroutine(OnRader());
        StartCoroutine(CoAttack());
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(TowerData.OffensePower, TowerData.Property);
        hitEffect.transform.position = -enemy.transform.up;
    }

    public override IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => target != null);

            StartCoroutine(SpikeAnimation());
            for (int i = 0; i < enemies.Count; i++)
            {
                if (i >= TowerData.attackTargetCount) break;

                if (enemies[i] != null)
                {
                    Attack(TowerData.OffensePower, enemies[i].healthSystem);
                }
            }

            yield return new WaitForSeconds(1f / TowerData.AttackSpeed);
        }
    }

    IEnumerator SpikeAnimation()
    {
        spriteObj.sprite = onSpike;
        yield return new WaitForSeconds(1f / TowerData.AttackSpeed / 2); 
        spriteObj.sprite = offSpike;
        yield return new WaitForSeconds(1f / TowerData.AttackSpeed / 2); 
    }
}