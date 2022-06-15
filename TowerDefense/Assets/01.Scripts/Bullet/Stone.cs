using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Bullet
{
    float maxTime = 1f;
    float curTime = 0f;

    Vector2 curvePoint = Vector2.zero;
    Vector2 startPos = Vector2.zero;
    Vector2 targetPos = Vector2.zero;

    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData, enemyTrm);
        IsShoot = true;
        curTime = 0f;
        startPos = transform.position;

        float x = (Target.transform.position.x + startPos.x) / 2;
        float y = targetPos.y > startPos.y ? targetPos.y : startPos.y;
        curvePoint = new Vector2(x, y + 2);

        // 28,29번 줄이 핵심! curTime = 0f, startPos = transform.position ㄱㄱ
        // 추가해야 하는 변수 -> curTime, maxTime, targetPos, startPos
        // Shoot 아래처럼 override 해서 쓰면 되여
        EnemyBase targetEnemy = Target.GetComponent<EnemyBase>();
        targetPos = GetTargetPos(targetEnemy.enemyData.MoveSpeed, targetEnemy.CurrentWayPointIndex, maxTime);
    }

    public override void Shoot()
    {
        transform.position = BezierCurves(startPos, curvePoint, Target.transform.position, curTime / maxTime);
        curTime += Time.deltaTime;
    }
}
