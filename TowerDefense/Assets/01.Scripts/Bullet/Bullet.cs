using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 20f;
    [SerializeField] private ParticleSystem effect = null;

    public Transform target { get; set; }
    public float bulletDamage { get; set; }
    private bool _isDead = false; // 총알이 죽었는가?
    

    void Update()
    {
        if (target != null && _isDead == false) // 타겟이 있고 죽은 상태가 아니라면
        {
            GuidedBullet();

            if (DistanceBullet())
            {
                CollisionBullet();
            }
        }
        else if (target == null)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnEnable()
    {
        _isDead = false;
    }

    void GuidedBullet()
    {
        try
        {
            //transform.LookAt(target);
            transform.position += (target.position - transform.position).normalized * speed * Time.deltaTime;
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            Debug.Log("ㅈㅈ");
        }
    }

    bool DistanceBullet()
    {
        if (Vector3.Distance(transform.position, target.position) <= 0.3f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void CollisionBullet()
    {
        target.gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage);
        _isDead = true;
        var ps = Instantiate(effect);
        ps.transform.position = target.position;
        ps.Play();

        gameObject.SetActive(false);
    } 
}
