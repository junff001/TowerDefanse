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

    //private float GetAngleFromVector(Vector3 dir)
    //{
    //    float radians = Mathf.Atan2(dir.y, dir.x);
    //    float degrees = radians * Mathf.Rad2Deg;
    //
    //    return degrees;
    //}
}
