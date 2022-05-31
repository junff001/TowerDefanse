using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Attack : CoreBase
{
    public override void Attack(int power, HealthSystem enemy)
    {
        Bomb bullet = PoolManager.GetItem<Bomb>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
