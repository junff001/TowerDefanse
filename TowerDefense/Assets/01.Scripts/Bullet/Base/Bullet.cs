using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    [SerializeField] protected float speed = 0f;                             // 이동 속도
    [SerializeField] protected ParticleSystem hitEffect = null;              // 타격 이펙트

    public Transform target = null;                                          // 목표물
    public int bulletDamage { get; set; } = 0;                               // 데미지
    public Define.PropertyType propertyType = Define.PropertyType.NONE;

    public void OnEnable()
    {
        Debug.Log("ㅎㅇㅎㅇ");
    }

    public virtual void Init(TowerData towerData)
    {
        propertyType = towerData.property;
        bulletDamage = towerData.OffensePower;
        this.gameObject.SetActive(true);
    }

    public virtual void Update()
    {
        if (target != null) 
        {
            Shoot();

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

    // 기본 유도탄
    public virtual void Shoot()
    {
        transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
    }

    // 거리 충돌 체크
    public virtual bool IsCollision()
    {
        return Vector2.Distance(transform.position, target.position) <= 0.1f ? true : false;
    }

    // 충돌 시 발생 로직
    public virtual void CollisionEvent()
    {
        if (target != null)
        {
            target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage, propertyType);
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
