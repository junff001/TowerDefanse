using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult_Attack : CoreBase
{ 
    public override void Attack(int power, HealthSystem enemy)
    {
        CannonBall bullet = Managers.Pool.GetItem<CannonBall>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
    }
}
