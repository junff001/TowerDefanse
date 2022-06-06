using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicBase : MonoBehaviour
{
    public Vector3 targetPos; // 뭔가 타겟 지점

    public virtual void Update()
    {
        if (IsCollision())
        {
            CollisionEvent();
        }
    }

    // 거리 충돌 체크
    public virtual bool IsCollision()
    {
        return Vector2.Distance(transform.position, targetPos) <= 0.1f ? true : false;
    }

    // 충돌 시 발생 로직
    public virtual void CollisionEvent()
    {

    }
}
