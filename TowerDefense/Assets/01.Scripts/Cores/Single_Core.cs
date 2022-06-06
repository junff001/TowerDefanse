using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Single_Core : CoreBase
{
    [SerializeField] private Transform head = null;
    [SerializeField] private SpriteRenderer body_forward = null;
    [SerializeField] private SpriteRenderer body_back = null;
    [SerializeField] private float angleSpeed = 0f;

    private SpriteRenderer haedSprite = null;
    private Bullet bullet = null;

    // 대포 회전 변수
    private float rotationZ;
    private const float maxValue = 90;
    private const float minValue = -80;


    void Awake()
    {
        haedSprite = head.GetComponent<SpriteRenderer>();
    }

    public override void Start()
    {
        base.Start();
        StartCoroutine(RotateCanon(angleSpeed));
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        if (bullet != null)
        {
            bullet.transform.position = transform.position;
            bullet.target = enemy.transform;
            bullet.bulletDamage = power;

            bullet.Init();
        }
        else
        {
            bullet = Managers.Pool.GetItem<Arrow>();

            bullet.transform.position = transform.position;
            bullet.target = enemy.transform;
            bullet.bulletDamage = power;

            bullet.Init();
        }

        bullet = null;
    }

    // 대포 머리 회전
    IEnumerator RotateCanon(float angleSpeed)
    {
        while (true)
        {
            if (currentTarget != null)
            {
                Vector2 direction = currentTarget.transform.position - head.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle - 45, Vector3.forward);
                float slerp = Quaternion.Slerp(head.rotation, rotation, angleSpeed * Time.deltaTime).z;
                head.rotation = Quaternion.Euler(0, 0, Mathf.Clamp(slerp, minValue, maxValue));

                if (rotationZ > maxValue || rotationZ < minValue)
                {
                    Flip(haedSprite, body_forward, body_back);
                }
            }

            yield return null;
        }
    }

    // 대포 스프라이트 플립X
    void Flip(SpriteRenderer head, SpriteRenderer body_forward, SpriteRenderer body_back)
    {
        if (head.flipY)
        {
            head.flipY = false;
            body_forward.flipX = false;
            body_back.flipX = false;
        }
        else
        {
            head.flipY = true;
            body_forward.flipX = true;
            body_back.flipX = true;
        }
        
        // 스프라이트 교정
        body_back.transform.localPosition *= new Vector2(-1, 1);
    }
}
