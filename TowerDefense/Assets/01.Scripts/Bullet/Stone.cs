using UnityEngine;

public class Stone : Bullet
{
    Vector2 curvePoint = Vector2.zero;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        IsShoot = true;

        targetPos = GetExpectPos(Target.GetComponent<EnemyBase>(), maxTime);

        float x = (Target.transform.position.x + startPos.x) / 2;
        float y = targetPos.y > startPos.y ? targetPos.y : startPos.y;
        y += 2;
        curvePoint = new Vector2(x, y);
    }

    public override void Shoot()
    {
        transform.position = BezierCurves(startPos, curvePoint, targetPos, curTime / maxTime);
    }
}
