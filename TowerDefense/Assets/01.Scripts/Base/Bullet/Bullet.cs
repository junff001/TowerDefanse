using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;                      // 속도
    public ParticleSystem hitEffect = null;        // 타격 이펙트

    public Transform target { get; set; } = null;                    // 타겟
    public int damage { get; set; } = 0;                             // 데미지

    public virtual void Update()
    {
        if (target != null)
        {
            FlyBullet();

            if (IsCollision())
            {
                HitEvent();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    // 투사체 충돌 체크 (거리)
    public virtual bool IsCollision()
    {
        return Vector3.Distance(transform.position, target.position) <= 0.1f ? true : false;
    }

    // 투사체 날아가는 함수
    public virtual void FlyBullet()
    {
        transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
    }

    // 투사체 충돌 시 처리 함수
    public virtual void HitEvent()
    {
        //target.gameObject.GetComponent<HealthSystem>().TakeDamage(damage);
        var ps = Instantiate(hitEffect);
        ps.transform.position = target.position;
        ps.Play();

        gameObject.SetActive(false);
    }
}
