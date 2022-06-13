using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    public Vector3 currentTarget;
    private Vector3 targetCatchPos = Vector3.zero;      // 투사 지점
    Vector3 moveDirection = Vector3.zero;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        targetCatchPos = currentTarget;
    }

    public override void Update()
    {
        if (isShoot)
        {
            Shoot();

            if (IsCollision())
            {
                moveDirection = Vector3.zero;
                CollisionEvent();

                target = null;
            }
        }
    }

    public override void Shoot()
    {
        transform.position += (targetCatchPos - transform.position).normalized * speed * Time.deltaTime;     
    }

    private float GetAngleFromVector(Vector3 dir)
    {
        float radians = Mathf.Atan2(dir.y, dir.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    public override bool IsCollision()
    {
        return Vector2.Distance(targetCatchPos, transform.position) <= 0.1f ? true : false;
    }

    public override void CollisionEvent()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage, propertyType, true);
            var ps = Instantiate(hitEffect);
            ps.transform.position = target.position;
            ps.Play();
            isShoot = false;

            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
