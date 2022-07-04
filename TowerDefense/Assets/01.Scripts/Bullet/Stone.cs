using UnityEngine;

public class Stone : Bullet
{
    Vector2 curvePoint = Vector2.zero;

    public override void InitProjectileData(float damage, Transform enemyTrm, BuffBase buff)
    {
        base.InitProjectileData(damage, enemyTrm, buff);
        IsShoot = true;

        targetPos = GetExpectPos(Target.GetComponent<Enemy>(), maxTime);

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
