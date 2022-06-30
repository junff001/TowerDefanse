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
        StartCoroutine(SetEnemyCorouitne());
        StartCoroutine(AttackDelay());
    }

    public override void Attack(float power, HealthSystem enemy)
    {
        enemy.GetComponent<EnemyBase>().AddBuff(Buff);
        enemy.TakeDamage(TowerData.AttackPower);
        hitEffect.transform.position = - enemy.transform.up;
    }

    public override IEnumerator AttackDelay()
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
                    Attack(TowerData.AttackPower, enemies[i].healthSystem);
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