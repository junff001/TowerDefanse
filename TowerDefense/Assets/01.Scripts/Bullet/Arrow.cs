using UnityEngine;

public class Arrow : Bullet
{
    public override void InitProjectileData(float damage, Transform enemyTrm, BuffBase buff)
    {
        base.InitProjectileData(damage, enemyTrm, buff);
        maxTime = Vector2.Distance(targetPos, startPos) / 10 * _maxTime; // 수정필요
        targetPos = GetExpectPos(enemyTrm.GetComponent<Enemy>(), maxTime);

        if (Target.GetComponent<Enemy>().IsDead) Target = null;
    }

    public override void CollisionEvent()
    {
        if (Target != null)
        {
            Debug.Log("버프");
            Target.GetComponent<Enemy>().AddBuff(Buff);
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
