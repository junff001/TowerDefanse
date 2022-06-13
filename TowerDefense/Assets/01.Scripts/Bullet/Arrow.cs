using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    Vector2 moveDir = Vector2.zero;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        moveDir = (target.transform.position - transform.position).normalized;
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
        if (target != null)
        {
            transform.rotation = Quaternion.Euler(0, 0, GetAngleFromVector(moveDir));
            transform.position += (target.transform.position - transform.position).normalized * speed * Time.deltaTime;
        }
    }

    public override bool IsCollision()
    {
        return Vector2.Distance(target.transform.position, transform.position) <= 0.1f ? true : false;
    }

    private float GetAngleFromVector(Vector3 dir)
    {
        float radians = Mathf.Atan2(dir.y, dir.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public override void CollisionEvent()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage, propertyType, true);
            var ps = Instantiate(hitEffect);
            ps.transform.position = target.position;
            ps.Play();
        }

        isShoot = false;
        gameObject.SetActive(false);
        target = null;
    }
}
