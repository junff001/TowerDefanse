using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_Core : CoreBase
{
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
                    if (enemies[i].GetComponent<HealthSystem>().canDamaged)
                    {
                        enemies[i].GetComponent<HealthSystem>().damagedDelay = towerData.AttackSpeed;
                        Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                        enemies[i].GetComponent<HealthSystem>().canDamaged = false;
                    } 
                }
            }

            yield return null;
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);
    }
}
