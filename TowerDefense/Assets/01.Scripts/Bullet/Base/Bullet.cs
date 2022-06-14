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
        if (Target != null) 
        {
            if (IsShoot)
            {
                Shoot();

                if (IsCollision())
                {
                    CollisionEvent();
                }
            }        
        }
        else
        {
            gameObject.SetActive(false);
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
        return Vector2.Distance(transform.position, Target.position) <= 0.1f ? true : false;
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
}
