using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : Bullet
{
    private Vector2 targetCatchPos = Vector2.zero;                  // 타겟 포착 위치
    private Vector2 projectilePos = Vector2.zero;                   // 발사체 위치
                                                                    
    [SerializeField] private float curveHeight = 0f;                // 커브 포인트 높이
    [SerializeField] private float explosionRadius = 0f;            // 폭발 반경
    [SerializeField] private float timerMax = 10f;

    private float timerCurrent = 0f;
    private Collider2D[] enemies = null;                            // 충돌 당한 적 리스트
    
    void Start()
    {
        targetCatchPos = target.position;
        projectilePos = transform.position;
    }

    public override void Update()
    {
        if (target != null)
        {
            if (timerCurrent > timerMax)
            {
                return;
            }

            timerCurrent += Time.deltaTime * speed;
            FlyBullet();

            if (IsCollision())
            {
                CollisionEvent();

                if (EnemiesInExplosionRaidus().Length > 0)
                {
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i].gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage);
                    }
                }
            }
        }
        else if (target == null)
        {
            gameObject.SetActive(false);
        }
    }

    public override void FlyBullet()
    {
        Vector2 midleValue = Vector2.Lerp(projectilePos, targetCatchPos, 0.5f);
        Vector2 curve = Vector2.Perpendicular(midleValue).normalized;
        
        Debug.Log("중간값 : " +  midleValue);
        Debug.Log("커브 값 : " + curve);

        transform.position = BezierCurves(projectilePos, curve, targetCatchPos, timerCurrent / timerMax);
    }
    
    Vector2 BezierCurves(Vector2 startPos, Vector2 curve, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curve, t);
        Vector2 lerp2 = Vector2.Lerp(curve, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }

    public override bool IsCollision()
    {
        return Vector2.Distance(transform.position, targetCatchPos) <= 0.1f ? true : false;
    }

    Collider2D[] EnemiesInExplosionRaidus()
    {
        enemies = Physics2D.OverlapCircleAll(targetCatchPos, explosionRadius);
        return enemies;
    }

    public override void CollisionEvent()
    { 
        var ps = Instantiate(hitEffect);
        ps.transform.position = targetCatchPos;
        ps.Play();

        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (UnityEditor.Selection.activeObject == gameObject)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(targetCatchPos, explosionRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
