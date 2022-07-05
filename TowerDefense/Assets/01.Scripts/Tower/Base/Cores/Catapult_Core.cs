using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult_Core : CoreBase
{
    [Header("파츠 변수")]
    [SerializeField] Transform head;
    [SerializeField] Transform basket;

    [Header("각도 변수")]
    [SerializeField] float defaultAngle;
    [SerializeField] float chargingAngle;
    [SerializeField] float followThroughAngle;
    [SerializeField] float throwAngle;

    [Header("속도 변수")]
    [SerializeField] float chargingSpeed;
    [SerializeField] float releaseSpeed;

    Vector3 dir = Vector3.zero;

    bool isThrowing = false;

    public override void OnEnable()
    {
        base.OnEnable();
        head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle);
    }
    void Update()
    {
        if (target != null)
        {
            dir = (target.transform.position - transform.position).normalized;

            if(dir.x > 0)
            {
                transform.rotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 180, 0);
            }
        }
    }

    public override void Attack(HealthSystem enemy, LayerMask opponentLayer)
    {

        if (bullet == null && false == isThrowing && target != null)
        {
            Ready();
            StartCoroutine(Charging(enemy.transform, opponentLayer));
        }  
    }

    void Ready()
    {
        bullet = Managers.Pool.GetItem<Stone>();
        bullet.transform.SetParent(basket);
        bullet.transform.localPosition = new Vector3(0, 0, 0);
        isThrowing = true;
    }

    IEnumerator Charging(Transform enemyTrm, LayerMask opponentLayer)
    {
        if(target != null)
        {
            while (true)
            {
                Quaternion charging = Quaternion.Euler(0, 0, chargingAngle - head.transform.localRotation.z);
                float t = chargingSpeed * Time.deltaTime;
                head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, charging, t);

                if (head.transform.localRotation == charging)
                {
                    StartCoroutine(Throw(enemyTrm, opponentLayer));
                    break;
                }
                else
                {
                    yield return null;
                }
            }
        }
        else
        {
            yield return null;
        }
        
    }

    IEnumerator Throw(Transform enemyTrm, LayerMask opponentLayer)
    {
        while (true)
        {
            Quaternion followThrough = Quaternion.Euler(0, 0, followThroughAngle - head.transform.localRotation.z);
            float t = releaseSpeed * Time.deltaTime;
            head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, followThrough, t);

            if (Mathf.Abs(head.transform.eulerAngles.z) <= throwAngle && bullet != null)
            {
                bullet.transform.SetParent(Managers.Pool.poolInitPos);
                bullet.transform.position = basket.transform.position + new Vector3(dir.x / 2, 0, 0);
                bullet.InitProjectileData(enemyTrm, Buff, opponentLayer);
                OnAttack();

                if (target?.healthSystem.GetAmount(eHealthType.HEALTH) - towerData.AttackPower <= 0) target = null;
            }

            if (head.transform.localRotation == followThrough)
            {
                head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle - head.transform.localRotation.z);
                isThrowing = false;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
}
