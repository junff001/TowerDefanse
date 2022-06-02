using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult_Core : CoreBase
{ 
    public override void Attack(int power, HealthSystem enemy)
    {
        CanonBall bullet = Managers.Pool.GetItem<CanonBall>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
