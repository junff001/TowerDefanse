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

            
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);
    }
}
