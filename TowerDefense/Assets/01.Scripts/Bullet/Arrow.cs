using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    private Vector3 targetCatchPos = Vector3.zero;      // 투사 지점
    Vector3 moveDirection = Vector3.zero;

    private void OnEnable()
    {
        if (target != null)
        {
            Debug.Log("공격을 시작합니다");
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
        FlyBullet();

        if (IsCollision())
        {
            moveDirection = Vector3.zero;
            target = null;
            CollisionEvent();
        }
    }

    public override void FlyBullet()
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
}
