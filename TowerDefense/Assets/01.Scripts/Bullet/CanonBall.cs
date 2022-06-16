using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanonBall : Bullet
{
    public  void OnEnable()
    {
        IsShoot = true;
    }

    public override void Shoot()
    {
        if(Target != null)
        {
            transform.position += (Target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    public override bool IsCollision()
    {
        return Vector2.Distance(transform.position, Target.transform.position) <= 0.2f && IsShoot;
    }
}
