using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Penetration_Attack : CoreBase
{
    [SerializeField] private Transform bowBody = null;         
    [SerializeField] private Transform bowLauncher = null;     
    [SerializeField] private float rotateSpeed = 0f;
    [SerializeField] private float pullTime = 0f;

    private Arrow bullet = null;
    private Animator animator = null;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public override void Start()
    {
        StartCoroutine(OnRader());
        StartCoroutine(OnAttack());
        StartCoroutine(LookAtTarget());
    }

    IEnumerator LookAtTarget()
    {
        while (true)
        {
            if (currentTarget != null)
            {
                Vector2 direction = currentTarget.transform.position - bowBody.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                bowBody.rotation = Quaternion.Slerp(bowBody.rotation, rotation, rotateSpeed * Time.deltaTime);
            }

            yield return null;
        }
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            for (int i = 0; i < enemies.Length; i++)
            {
                if (i >= towerData.attackTargetCount)
                    break;

                if (enemies[i] != null)
                {
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                }
            }

            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet != null)
        {
            bullet.transform.position = bowLauncher.position;
            bullet.target = enemy.transform;
            bullet.bulletDamage = power;

            bullet.Init();
        }
        else
        {           
            bullet = Managers.Pool.GetItem<Arrow>();

            bullet.transform.position = bowLauncher.position;
            bullet.target = enemy.transform;
            bullet.bulletDamage = power;

            bullet.Init();
        }

        bullet = null;
    }
}
