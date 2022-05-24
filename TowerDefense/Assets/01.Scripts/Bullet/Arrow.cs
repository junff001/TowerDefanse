using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : Bullet
{
    private Vector3 targetCatchPos = Vector3.zero;      // 투사 지점
    private Vector2 lastMoveDir = Vector2.zero;         // 마지막 방향
    private bool isWaiting = true;                      // 대기 중

    void Start()
    {
        if (target != null)
        {
             targetCatchPos = target.position;
        }
    }

    public override void Update()
    {
        isWaiting = target == null ? true : false;

        if (!isWaiting)
        {
            FlyBullet();
                
            if (IsCollision())
            {
                CollisionEvent();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }

    }

    public override void FlyBullet()
    {
        Vector3 moveDirection = (targetCatchPos - transform.position).normalized;
        transform.position += moveDirection * speed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(moveDirection) -45);
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
