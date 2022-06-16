using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : Bullet
{
    public override void OnEnable()
    {
        base.OnEnable();
        IsShoot = true;
    }

    public override void Shoot()
    {
        transform.position += (Target.transform.position - transform.position).normalized * speed * Time.deltaTime;
    }
}
