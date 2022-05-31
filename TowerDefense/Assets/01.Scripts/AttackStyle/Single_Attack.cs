using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Attack : CoreBase
{ 
    public override void Attack(int power, HealthSystem enemy)
    {
        CannonBall bullet = PoolManager.GetItem<CannonBall>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
