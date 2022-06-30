using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration_Core : CoreBase
{
    [Header("파츠")]
    [SerializeField] private Transform bowBody = null;         
    [SerializeField] private Transform bowLauncher = null;
    [SerializeField] private Transform bulletLanchTrm = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    [Header("스프라이트")]
    [SerializeField] private Sprite bow = null;
    [SerializeField] private Sprite pullBow = null;

    [Header("시간/속도")]
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private float TransitionTime = 0f;

    //private Arrow bullet = null;
    
    public override void OnEnable()
    {
        base.OnEnable();

        StartCoroutine(LookAtTarget());
    }

    public override void OnDisable()
    {
        base.OnDisable();

        StopCoroutine(LookAtTarget());
    }

    IEnumerator LookAtTarget()
    {
        while (true)
        {
            if (target != null && enemies.Count > 0)
            {
                Vector2 direction = target.transform.position - bowBody.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                transform.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);

                if (bullet != null)
                {
                    bullet.transform.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);
                }
            }

            yield return null;
        }
    }

    public override void Attack(float power, HealthSystem enemy)
    {
        if (bullet == null && target != null)
        {
            Ready();
            StartCoroutine(PullAndShoot(TransitionTime, enemy));
        }
    }

    IEnumerator PullAndShoot(float TransitionTime, HealthSystem enemy)
    {
        if(target != null && enemy.IsDead() == false)
        {
            yield return new WaitForSeconds(TransitionTime);
            Pull();
            yield return new WaitForSeconds(TransitionTime);
            Shoot();
            yield return new WaitForSeconds(TransitionTime);
            OnAttack();
        }
        else
        {
            yield return null;
        }
        
    }

    void Ready()
    {
        bullet = Managers.Pool.GetItem<Arrow>();
        bullet.transform.position = bulletLanchTrm.position;
        bullet.transform.SetParent(bulletLanchTrm);
        bullet.transform.localPosition = new Vector2(0.3f, 0.3f);
    }

    void Pull()
    {
        spriteRenderer.sprite = pullBow;
        bowLauncher.localPosition = new Vector2(-0.5f, -0.5f); 
    }

    void Shoot()
    {
        bowLauncher.localPosition = new Vector2(0, 0);
        if (target != null && target.GetComponent<EnemyBase>().IsDead == false)
        {
            bullet.InitProjectileData(TowerData.AttackPower, target.transform, Buff); // targetPos 세팅 됐을거라 믿고?

            Vector2 direction = bullet.TargetPos - (Vector2)bowBody.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            bowBody.rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
            bullet.IsShoot = true;
        }
        else
        {
            bullet.gameObject.SetActive(false);
        }

        spriteRenderer.sprite = bow;
    }
}
