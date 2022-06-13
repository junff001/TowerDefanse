using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration_Core : CoreBase
{
    [Header("파츠")]
    [SerializeField] private Transform bowBody = null;         
    [SerializeField] private Transform bowLauncher = null;
    [SerializeField] private SpriteRenderer spriteRenderer = null;

    [Header("스프라이트")]
    [SerializeField] private Sprite bow = null;
    [SerializeField] private Sprite pullBow = null;

    [Header("시간/속도")]
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private float TransitionTime = 0f;

    private Arrow bullet = null;
    
    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LookAtTarget());
    }

    IEnumerator LookAtTarget()
    {
        while (true)
        {
            if (currentTarget != null && enemies.Length > 0)
            {
                Vector2 direction = currentTarget.transform.position - bowBody.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                bowBody.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);

                if (bullet != null)
                {
                    bullet.transform.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);
                }
            }

            yield return null;
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet == null && currentTarget != null)
        {
            Ready();
            StartCoroutine(PullAndShoot(TransitionTime, enemy));
        }
    }

    IEnumerator PullAndShoot(float TransitionTime, HealthSystem enemy)
    {
        yield return new WaitForSeconds(TransitionTime);
        Pull();
        yield return new WaitForSeconds(TransitionTime);
        Shoot();
        yield return new WaitForSeconds(TransitionTime);
        bullet = null;
        currentTarget = null;
    }

    void Ready()
    {
        bullet = Managers.Pool.GetItem<Arrow>();
        bullet.Init(towerData, currentTarget.transform);
        bullet.transform.position = bowLauncher.position;
        bullet.transform.SetParent(bowLauncher);
        bullet.transform.localPosition = new Vector2(0.3f, 0.3f);
    }

    void Pull()
    {
        spriteRenderer.sprite = pullBow;
        bowLauncher.localPosition = new Vector2(-0.5f, -0.5f); 
    }

    void Shoot()
    {
        bullet.transform.SetParent(null);
        bowLauncher.localPosition = new Vector2(0, 0);
        bullet.isShoot = true;
        spriteRenderer.sprite = bow;
    }
}
