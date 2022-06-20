using UnityEngine;

public class Arrow : Bullet
{
    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        maxTime = Vector2.Distance(targetPos, startPos) / towerData.AttackRange * _maxTime;
        GetExpectPos(enemyTrm.GetComponent<EnemyBase>(), maxTime);

        if (Target.GetComponent<EnemyBase>().IsDead) Target = null;
    }

    // 충돌 시 발생 로직
    public override void CollisionEvent()
    {
        if (Target != null)
        {
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage, PropertyType, true);

            var ps = Instantiate(hitEffect);
            ps.transform.position = Target.position;
            ps.Play();
        }
        IsShoot = false;
        transform.SetParent(Managers.Pool.poolInitPos);
        gameObject.SetActive(false);
    }

    //private float GetAngleFromVector(Vector3 dir)
    //{
    //    float radians = Mathf.Atan2(dir.y, dir.x);
    //    float degrees = radians * Mathf.Rad2Deg;
    //
    //    return degrees;
    //}
}
