using UnityEngine;

public class Bomb : Bullet
{
    [SerializeField] float explosionRadius = 0f;           
    Collider2D[] enemies = null;
    public LayerMask enemyMask;
    Vector2 curvePoint = Vector2.zero;

    public override void Init(TowerData towerData, Transform enemyTrm, BuffBase buff)
    {
        base.Init(towerData, enemyTrm, buff);
        IsShoot = true;
        float x = (targetPos.x + startPos.x) / 2;
        float y = targetPos.y > startPos.y ? targetPos.y : startPos.y;
        y += 2;

        curvePoint = new Vector2(x, y);
    }

    public override void Update()
    {
        if (Target != null)
        {
            curTime += Time.deltaTime;
            Shoot();

            if (IsCollision())
            {
                CollisionEvent();

                enemies = Physics2D.OverlapCircleAll(targetPos, explosionRadius, enemyMask);
                if (enemies.Length > 0)
                {
                    for (int i = 0; i < enemies.Length; i++)
                    {
                        enemies[i].gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage);
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
        transform.position = BezierCurves(startPos, curvePoint, targetPos, curTime / maxTime);
    }

    public override void CollisionEvent()
    { 
        var ps = Instantiate(hitEffect);
        ps.transform.position = targetPos;
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
            Gizmos.DrawWireSphere(targetPos, explosionRadius);
            Gizmos.color = Color.white;
        }
    }
#endif
}
