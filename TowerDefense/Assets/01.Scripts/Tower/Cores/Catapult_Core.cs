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

    [Header("시간 변수")]
    [SerializeField] float TransitionTime;
    [SerializeField] float chargingTime;
    [SerializeField] float releaseTime;

    Stone bullet = null;

    public override void OnEnable()
    {
        base.OnEnable();

        head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle - head.transform.localRotation.z);
    }

    void Update()
    {
        if(target != null)
        {
            Vector3 dir = target.transform.position - transform.position;

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

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet == null)
        {
            Ready(enemy);
            StartCoroutine(Charging1());
        }  
    }

    void Ready(HealthSystem enemy)
    {
        bullet = Managers.Pool.GetItem<Stone>();
        bullet.Init(towerData, enemy.transform);
        bullet.transform.SetParent(basket);
        bullet.transform.localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator Charging1()
    {
        while (true)
        {
            Quaternion charging = Quaternion.Euler(0, 0, chargingAngle - head.transform.localRotation.z);
            float t = chargingSpeed * Time.deltaTime;
            head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, charging, t);

            if (head.transform.localRotation == charging)
            {
                Debug.Log("던지기");
                StartCoroutine(Throw1());
                break;    
            }
            else
            {
                yield return null;
            }
        } 
    }

    IEnumerator Throw1()
    {
        while (true)
        {
            Quaternion followThrough = Quaternion.Euler(0, 0, followThroughAngle - head.transform.localRotation.z);
            float t = releaseSpeed * Time.deltaTime;
            head.transform.localRotation = Quaternion.Slerp(head.transform.localRotation, followThrough, t);

            if (head.transform.localRotation.z <= throwAngle)
            {
                bullet.isShoot = true;
                bullet.transform.SetParent(Managers.Pool.poolInitPos);
                bullet = null;
            }

            if (head.transform.localRotation == followThrough)
            {
                head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle - head.transform.localRotation.z);
                bullet = null;
                break;
            }
            else
            {
                yield return null;
            }
        }
    }
}
