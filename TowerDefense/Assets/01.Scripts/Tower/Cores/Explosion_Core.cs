using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Explosion_Core : CoreBase
{
    public override void Attack(int power, HealthSystem enemy)
    {
        Bomb bullet = Managers.Pool.GetItem<Bomb>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.enemyMask = enemyMask;
        bullet.Init(towerData,enemy.transform);
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            Collider2D guardian = enemies.ToList().Find(x => x?.gameObject.layer == 1 << LayerMask.NameToLayer("GuardianEnemy")); // Guardian
            if (guardian == null)
            {
                for (int i = 0; i < enemies.Length; i++)
                {
                    if (i >= towerData.attackTargetCount)
                        break;

                    if (enemies[i] != null)
                    {
                        Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                    }
                }
            }
            else
            {
                Attack(towerData.OffensePower * enemies.Length, guardian.GetComponent<HealthSystem>());
            }


            yield return new WaitForSeconds(1f / towerData.AttackSpeed); // 怨듭냽留뚰겮 湲곕떎由ш퀬,
        }
    }
}
