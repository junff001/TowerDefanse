using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration_Attack : CoreBase
{
    [SerializeField] private Transform bowBody = null;         // 활대
    [SerializeField] private Transform bowLauncher = null;     // 런처
    [SerializeField] private float rotateSpeed = 0f;           // 회전 속도

    private Bullet bullet = null;

    public override void Start()
    {
        base.Start();

        bullet = PoolManager.GetItem<Bullet>();
        bullet.transform.position = bowLauncher.position;
    }

    void Update()
    {
        LookAtTarget();
    }
  
    void LookAtTarget()
    {
        if (currentTarget != null)
        {
            Vector2 direction = currentTarget.transform.position - bowBody.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
            bowBody.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);
        }
        else
        {
            Debug.Log("없음");
        }

    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet != null)
        {
            bullet.target = enemy.transform;
            bullet.bulletDamage = power;

            bullet.Init();

            bullet = PoolManager.GetItem<Bullet>();
            bullet.transform.position = bowLauncher.position;
        }
        //else
        //{
        //    bullet = PoolManager.GetItem<Bullet>();

        //    bullet.transform.position = bowLauncher.position;
        //    bullet.target = enemy.transform;
        //    bullet.bulletDamage = power;

        //    bullet.Init();
        //}

        //bullet = null;
    }
}
