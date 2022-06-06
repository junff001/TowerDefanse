using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinBarrel : MagicBase
{
    Vector3 direction = Vector3.zero;

    [SerializeField] private float timerMax = 3f; // 날아가는 시간?
    private float timerCurrent = 0f; // 현재 날아가는 중
    public float speed;


    private void Start()
    {
        direction = targetPos - transform.position;
        speed = direction.x / timerMax; // 가야 하는 거리를 총 시간으로 빼 봤음..
    }

    public override void Update()
    {
        base.Update();
        timerCurrent += Time.deltaTime * speed;
        Shoot();
    }

    public void Shoot()
    {
        Vector3 z = Vector3.forward;
        Vector3 curve = Vector3.Cross(direction, z);
        Vector3 pos = transform.position + direction / 2;
        Vector3 result = pos + curve.normalized * Vector3.Distance(targetPos, transform.position) / 2;
        transform.position = BezierCurves(transform.position, result, targetPos, timerCurrent / timerMax);
    }

    Vector2 BezierCurves(Vector2 startPos, Vector2 curve, Vector2 endPos, float t)
    {
        Vector2 lerp1 = Vector2.Lerp(startPos, curve, t);
        Vector2 lerp2 = Vector2.Lerp(curve, endPos, t);

        return Vector2.Lerp(lerp1, lerp2, t);
    }

    public override void CollisionEvent()
    {
        base.CollisionEvent(); 

        

    }
}
