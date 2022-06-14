using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    Vector2 moveDir = Vector2.zero;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        moveDir = (Target.transform.position - transform.position).normalized;
    }

    public override void Update()
    {
        if (isShoot)
        {
            if (IsCollision())
            {
                CollisionEvent();

            }
            Shoot();
        }
    }

    public override void Shoot()
    {
        if (Target != null)
        {
            transform.rotation = Quaternion.Euler(0, 0, GetAngleFromVector(moveDir));
            transform.position += (Target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    public override bool IsCollision()
    {
        if (Target == null) return true;
        return Vector2.Distance(Target.transform.position, transform.position) <= 0.1f ? true : false;
    }

    private float GetAngleFromVector(Vector3 dir)
    {
        float radians = Mathf.Atan2(dir.y, dir.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public override void CollisionEvent()
    {
        if (Target != null)
        {
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage, PropertyType, true);
            var ps = Instantiate(hitEffect);
            ps.transform.position = Target.position;
            ps.Play();
        }

        isShoot = false;
        gameObject.SetActive(false);
        Target = null;
    }
}
