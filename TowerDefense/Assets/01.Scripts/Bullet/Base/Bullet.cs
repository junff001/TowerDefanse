using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 0f;                             // 이동 속도
    [SerializeField] protected ParticleSystem hitEffect = null;              // 타격 이펙트

    public Transform Target = null;                                          // 목표물
    public int BulletDamage { get; set; } = 0;                               // 데미지
    public bool IsShoot { get; set; } = false;
    public Define.PropertyType PropertyType = Define.PropertyType.NONE;

    public virtual void Init(TowerData towerData, Transform enemyTrm)
    {
        Target = enemyTrm;
        PropertyType = towerData.Property;
        BulletDamage = towerData.OffensePower;
    }

    public virtual void Update()
    {
        if (IsShoot)
        {
            if (Target != null)
            {
                if (IsCollision())
                {
                    CollisionEvent();
                }
                Shoot();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    // 기본 유도탄
    public virtual void Shoot()
    {
        transform.position += (Target.position - transform.position).normalized * speed * Time.deltaTime;
    }

    // 거리 충돌 체크
    public virtual bool IsCollision()
    {
        return Vector2.Distance(transform.position, Target.position) <= 0.2f ? true : false;
    }

    // 충돌 시 발생 로직
    public virtual void CollisionEvent()
    {
        if (Target != null)
        {
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage, PropertyType);

            var ps = Instantiate(hitEffect);
            ps.transform.position = Target.position;
            ps.Play();
            IsShoot = false;    

            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    protected Vector2 GetTargetPos(float speed, int index, float maxTime)
    {
        //현재 목표 웨이포인트
        Vector2 destPoint = Managers.Game.wayPoints[index].position;
        //이동하고 있는 방향
        Vector2 movingDir = (destPoint - (Vector2)transform.position).normalized;
        // maxTime이 지난 후, 적의 위치
        Vector2 enemyPosLaterMaxTime = (Vector2)transform.position + (movingDir * speed * maxTime);

        // 예상 위치
        Vector2 expectPos = GetExpectPos(destPoint, movingDir, enemyPosLaterMaxTime);

        // 목표지점을 넘어선 경우인데, 다음 웨이포인트가 존재한다면.
        if (expectPos == Vector2.zero && Managers.Game.wayPoints.Count > (index + 1))
        {
            return GetTargetPos(speed, ++index, maxTime);
        }
        else
        {
            return expectPos;
        }
    }

    protected bool CheckPassedPoint(bool bMovingRight, float expect, float dest)
    {
        if (bMovingRight && false == expect >= dest) return true;
        else if (!bMovingRight && false == expect <= dest) return true;
        return false;
    }

    protected Vector2 GetExpectPos(Vector2 destPoint, Vector2 movingDir, Vector2 enemyPosLaterMaxTime)
    {
        if (movingDir.x != 0) // 옆으로 이동중
        {
            if (CheckPassedPoint(movingDir.x > 0, enemyPosLaterMaxTime.x, destPoint.x))
                return enemyPosLaterMaxTime;
        }
        else
        {
            if (CheckPassedPoint(movingDir.y > 0, enemyPosLaterMaxTime.y, destPoint.y))
                return enemyPosLaterMaxTime;
        }
        //여기까지 온거면, 목표지점을 넘어선 거임.
        return Vector2.zero;
    }

    protected Vector2 BezierCurves(Vector2 startPos, Vector2 curve, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curve, t);
        Vector2 lerp2 = Vector2.Lerp(curve, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }
}
