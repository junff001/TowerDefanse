using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    public Vector3 targetCatchPos = Vector3.zero;                  // 타겟 포착 위치
    public Vector3 projectilePos = Vector3.zero;                   // 발사체 위치
                                                                    
    [SerializeField] private float curveHeight = 0f;                // 커브 포인트 높이
    [SerializeField] private float explosionRadius = 0f;            // 폭발 반경
    [SerializeField] private float timerMax = 10f;

    private float timerCurrent = 0f;
    private Collider2D[] enemies = null;                            // 충돌 당한 적 리스트

    public LayerMask enemyMask = default;

    public override void Update()
    {
        if (target != null)
        {
            if (timerCurrent > timerMax)
            {
                return;
            }

            timerCurrent += Time.deltaTime * speed;
            Shoot();

            if (IsCollision())
            {
                CollisionEvent();

                enemies = EnemiesInExplosionRaidus();
                if (enemies.Length > 0)
                {
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i].gameObject.GetComponent<HealthSystem>().TakeDamage(bulletDamage,propertyType);
                    }
                }
            }
        }
        else if (target == null)
        {
            gameObject.SetActive(false);
        }
    }

    public override void Shoot()
    {
        /*        Vector3 direction = targetCatchPos - projectilePos;
                Vector3 z = Vector3.forward;
                Vector3 curve = Vector3.Cross(direction, z);

                Vector3 pos = projectilePos + direction / 2;
        
                Vector3 result = pos + curve.normalized * Vector3.Distance(targetCatchPos, projectilePos) / 2;
         */

        float x = (targetCatchPos.x + projectilePos.x) / 2;

        float y = targetCatchPos.y > projectilePos.y ? targetCatchPos.y : projectilePos.y;
        y += 1;

        Vector3 result = new Vector3(x, y, 0);
        
        transform.position = BezierCurves(projectilePos, result, targetCatchPos, timerCurrent / timerMax);
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
        return Physics2D.OverlapCircleAll(targetCatchPos, explosionRadius, enemyMask);
    }

    public override void CollisionEvent()
    { 
        var ps = Instantiate(hitEffect);
        ps.transform.position = targetCatchPos;
        ps.Play();

        target = null;
        gameObject.SetActive(false);
    }

    public override void Init(TowerData towerData)
    {
        base.Init(towerData);
        timerCurrent = 0;
        targetCatchPos = target.position;
        projectilePos = transform.position;
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
