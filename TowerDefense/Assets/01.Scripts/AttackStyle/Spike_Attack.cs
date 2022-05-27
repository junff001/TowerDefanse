using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike_Attack : CoreBase
{
    [SerializeField] private SpriteRenderer spriteObj = null;
    [SerializeField] private ParticleSystem hitEffect = null;
    [SerializeField] private Sprite onSpike = null;
    [SerializeField] private Sprite offSpike = null;

    public override void Start()
    {
        StartCoroutine(Rader());
        StartCoroutine(OnAttack());
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);

        hitEffect.transform.position = -enemy.transform.up;
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            for (int i = 0; i < enemies.Length; i++)
            {
                if (i >= towerData.attackTargetCount)
                    break;

                if (enemies[i] != null)
                {
                    spriteObj.sprite = onSpike;
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고
            spriteObj.sprite = offSpike;
            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 공속만큼 기다리고
        }
    }
}