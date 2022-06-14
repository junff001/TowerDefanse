using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Bullet
{
    // 조정 변수
    [SerializeField] float curveHeight = 0f;               
    [SerializeField] float explosionRadius = 0f;           
    [SerializeField] float timerMax = 10f;

    // 위치 변수
    Vector3 targetCatchPos = Vector3.zero;                
    Vector3 projectilePos = Vector3.zero;

    // 뒷받침 변수
    float timerCurrent = 0f;
    Collider2D[] enemies = null;

    // 외부 변수
    public LayerMask enemyMask;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        timerCurrent = 0;
        targetCatchPos = Target.position;
        projectilePos = transform.position;
    }

    public override void Update()
    {
        if (Target != null)
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
                        enemies[i].gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage,PropertyType);
                    }
                }
            }
        }
        else if (Target == null)
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

        Target = null;
        gameObject.SetActive(false);
    }

#if UNITY_EDITOR
    void OnDrawGizmos()
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
