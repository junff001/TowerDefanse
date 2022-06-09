using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult_Core : CoreBase
{
    [Header("파츠 변수")]
    [SerializeField] private Transform head = null;
    [SerializeField] private Transform basket = null;

    [Header("각도 변수")]
    [SerializeField] private float defaultAngle = 0f;
    [SerializeField] private float chargingAngle = 0f;
    [SerializeField] private float followThroughAngle = 0f;

    [Header("속도 변수")]
    [SerializeField] private float chargingSpeed = 0f;
    [SerializeField] private float releaseSpeed = 0f;   
    
    [Header("시간 변수")]
    [SerializeField] private float TransitionTime = 0f;
    [SerializeField] private float chargingTime = 0f;
    [SerializeField] private float releaseTime = 0f;

    private Stone bullet = null;

    public override void OnEnable()
    {
        base.OnEnable();

        head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle - head.transform.localRotation.z);
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet == null)
        {
            Ready(enemy);
            StartCoroutine(Charging(power, enemy));
            //StartCoroutine(ChargingAndThrow(TransitionTime, power, enemy));
        }  
    }

    void Ready(HealthSystem enemy)
    {
        Debug.Log("투사체 생성");
        bullet = Managers.Pool.GetItem<Stone>();
        bullet.Init(towerData,enemy.transform);
        bullet.transform.SetParent(basket);
        bullet.transform.localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator Charging(int power, HealthSystem enemy)
    {
        while (true)
        {
            Quaternion charging = Quaternion.Euler(0, 0, chargingAngle - head.transform.rotation.z);
            float t = chargingSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, charging, t);

            if (head.transform.rotation == charging)
            {
                StartCoroutine(Throw(enemy));
                break;
                
            }
            else
            {
                yield return null;
            }
        } 
    }

    IEnumerator Throw(HealthSystem enemy)
    {
        while (true)
        {
            Quaternion followThrough = Quaternion.Euler(0, 0, followThroughAngle - head.transform.rotation.z);
            float t = releaseSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, followThrough, t);

            if (head.transform.rotation == followThrough)
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

        if (bullet != null)
        {
            bullet.transform.SetParent(null);
        }
    }
}
