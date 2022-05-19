using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion_Attack : CoreBase
{
    public override void Attack(int power, HealthSystem enemy)
    {
        Missile bullet = PoolManager.GetItem<Missile>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
