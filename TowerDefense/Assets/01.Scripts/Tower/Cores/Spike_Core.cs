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
        EnemyBase enemyBase = enemy.GetComponent<EnemyBase>();

        if (false == enemyBase.enemyData.IsFlying)
        {
            enemy.TakeDamage(towerData.OffensePower, towerData.Property);

            hitEffect.transform.position = -enemy.transform.up;
        }
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);

            for (int i = 0; i < enemies.Length; i++)
            {
                if (i >= towerData.attackTargetCount)
                    break;

                if (enemies[i] != null)
                {
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                    StartCoroutine(SpikeAnimation());
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