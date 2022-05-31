using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    private Vector3 targetCatchPos = Vector3.zero;      // 투사 지점
    Vector3 moveDirection = Vector3.zero;

    public override void Init()
    {
        if (target != null)
        {
            targetCatchPos = target.position;
            moveDirection = (targetCatchPos - transform.position).normalized;
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(moveDirection) - 45);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public override void Update()
    {
        Shoot();

        if (IsCollision())
        {
            moveDirection = Vector3.zero;
            CollisionEvent();

            target = null;
        }
    }

    public override void Shoot()
    {
        transform.position += moveDirection * speed * Time.deltaTime;
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
            target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage, true);
            var ps = Instantiate(hitEffect);
            ps.transform.position = target.position;
            ps.Play();

            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
