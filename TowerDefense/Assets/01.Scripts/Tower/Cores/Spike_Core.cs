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
        StartCoroutine(OnAttack());
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(towerData.OffensePower, towerData.Property);
        hitEffect.transform.position = -enemy.transform.up;
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies.Count > 0);

            StartCoroutine(SpikeAnimation());
            for (int i = 0; i < enemies.Count; i++)
            {
                if (i >= towerData.attackTargetCount) break;

                if (enemies[i] != null)
                {
                    Attack(towerData.OffensePower, enemies[i].healthSystem);
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
    }

    IEnumerator SpikeAnimation()
    {
        spriteObj.sprite = onSpike;
        yield return new WaitForSeconds(1f / towerData.AttackSpeed / 2); 
        spriteObj.sprite = offSpike;
        yield return new WaitForSeconds(1f / towerData.AttackSpeed / 2); 
    }
}