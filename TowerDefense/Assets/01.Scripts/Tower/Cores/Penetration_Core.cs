using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration_Core : CoreBase
{
    [SerializeField] private Transform bowBody = null;         
    [SerializeField] private Transform bowLauncher = null;
    [SerializeField] private Sprite bow = null;
    [SerializeField] private Sprite pullBow = null;
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private float TransitionTime = 0f;

    private Arrow bullet = null;
    [SerializeField] private SpriteRenderer sr = null;

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LookAtTarget());

        Pull();
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
        if (bullet == null)
        {
            Ready(enemy.transform);
            StartCoroutine(PullAndShoot(TransitionTime, enemy));
        }
    }

    IEnumerator PullAndShoot(float TransitionTime, HealthSystem enemy)
    {
        yield return new WaitForSeconds(TransitionTime);
        Pull();
        yield return new WaitForSeconds(TransitionTime);
        Shoot(towerData.OffensePower, enemy);
        yield return new WaitForSeconds(TransitionTime);
        bullet = null;
    }

    void Ready(Transform enemyTrm)
    {
        bullet = Managers.Pool.GetItem<Arrow>();
        bullet.Init(towerData,enemyTrm);
        bullet.transform.position = bowLauncher.position;
        bullet.transform.SetParent(bowLauncher);
        bullet.transform.localPosition = new Vector2(0.3f, 0.3f);
    }

    void Pull()
    {
        sr.sprite = pullBow;
        bowLauncher.localPosition = new Vector2(-0.5f, -0.5f); 
        bullet.transform.localPosition = new Vector2(0.5f, 0.5f);
    }

    void Shoot(int power, HealthSystem enemy)
    {       
        bowLauncher.localPosition = new Vector2(0, 0);
        bullet.transform.SetParent(null);
        bullet.isShoot = true;
        sr.sprite = bow;
    }
}
