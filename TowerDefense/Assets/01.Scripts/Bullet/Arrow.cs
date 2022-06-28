using UnityEngine;

public class Arrow : Bullet
{
    public override void Init(TowerData towerData, Transform enemyTrm, BuffBase buff)
    {
        base.Init(towerData, enemyTrm, buff);
        maxTime = Vector2.Distance(targetPos, startPos) / towerData.AttackRange * _maxTime;
        targetPos = GetExpectPos(enemyTrm.GetComponent<EnemyBase>(), maxTime);

        if (Target.GetComponent<EnemyBase>().IsDead) Target = null;
    }

    public override void CollisionEvent()
    {
        if (Target != null)
        {
            Debug.Log("버프");
            Target.GetComponent<EnemyBase>().AddBuff(Buff);
            Target.gameObject.GetComponent<HealthSystem>().TakeDamage(BulletDamage, true);

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
