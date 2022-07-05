using UnityEngine;

public class Stone : Projectile
{
    Vector2 curvePoint = Vector2.zero;

    public override void InitProjectileData(Transform enemyTrm, BuffBase buff, LayerMask opponent)
    {
        base.InitProjectileData(enemyTrm, buff, opponent);
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
