using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Catapult_Core : CoreBase
<<<<<<< HEAD
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

    public override void Start()
    {
        base.Start();

        head.transform.localRotation = Quaternion.Euler(0, 0, defaultAngle - head.transform.localRotation.z);
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet == null)
        {
            Ready();
            StartCoroutine(Charging());
            //StartCoroutine(ChargingAndThrow(TransitionTime, power, enemy));
        }  
    }

    void Ready()
    {
        bullet = Managers.Pool.GetItem<Stone>();
        bullet.transform.SetParent(basket);
        bullet.transform.localPosition = new Vector3(0, 0, 0);
    }

    IEnumerator ChargingAndThrow(float TransitionTime, int power, HealthSystem enemy)
    {
        //yield return new WaitForSeconds(TransitionTime);
        StartCoroutine(Charging());
        //yield return new WaitForSeconds(TransitionTime);
        //StartCoroutine(Throw(power, enemy));
        //yield return new WaitForSeconds(TransitionTime);
        yield return null;
        bullet = null;
    }

    IEnumerator Charging()
    {
        while (true)
        {
            Quaternion charging = Quaternion.Euler(0, 0, chargingAngle - head.transform.rotation.z);
            float t = chargingSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, charging, t);

            if (head.transform.rotation == charging)
            {
                StartCoroutine(Throw());
                break;
                
            }
            else
            {
                yield return null;
            }
        } 
    }

    IEnumerator Throw()
    {
        while (true)
        {
            Quaternion followThrough = Quaternion.Euler(0, 0, followThroughAngle - head.transform.rotation.z);
            float t = releaseSpeed * Time.deltaTime;
            head.transform.rotation = Quaternion.Slerp(head.transform.rotation, followThrough, t);

            if (head.transform.rotation == followThrough)
            {
                break;
            }
            else
            {
                yield return null;
            }
        }
       
        //bullet.target = enemy.transform;
        //bullet.bulletDamage = power;
        //bullet.transform.SetParent(null);
        //bullet.Init();
=======
{ 
    public override void Attack(int power, HealthSystem enemy)
    {
        CanonBall bullet = Managers.Pool.GetItem<CanonBall>();

        bullet.transform.position = transform.position;
        bullet.target = enemy.transform;
        bullet.bulletDamage = power;
>>>>>>> 2e16e90759d7fbe0fd3f2af2d3173c68aabfcb97
    }
}