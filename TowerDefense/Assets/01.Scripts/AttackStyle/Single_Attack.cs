using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Attack : CoreBase
{ 
    public override void Attack(int power, HealthSystem enemy)
    {
        Bullet bullet = PoolManager.GetItem<Bullet>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
