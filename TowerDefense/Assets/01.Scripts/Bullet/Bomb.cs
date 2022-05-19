using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


// 미래의 나. 넌 베지어 곡선의 시작점과 끝점의 중간에 위치할 2개의 커브 점의 위치를 공식(비례식)으로 구해내야 돼
// 첫 번째 방범은 사잇값이야

public class Bomb : Bullet
{
    [SerializeField] private float distanceFromStartPos = 0f;                      // 시작 포인트로 부터 얼마나 얼마나 꺾을지
    [SerializeField] private float curveHeghit = 0f;

    private Vector3[] bezierPos = new Vector3[3];
    private float timerMax = 1f;
    private float timerCurrent = 0f;
    private Vector2 targetCatchPos = Vector2.zero;                                 // 포착 시 적 위치

    void Start()
    {
        targetCatchPos = target.position;
    }

    public override void Update()
    {
        if (target != null)
        {
            if (timerCurrent > timerMax)
            {
                return;
            }

            timerCurrent += Time.deltaTime * speed;

            FlyBullet(); 

            if (IsCollision())
            {
                HitEvent();
            }
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public override bool IsCollision()
    {
        return Vector3.Distance(transform.position, targetCatchPos) <= 0.1f ? true : false;
    }

    public override void FlyBullet()
    {
        Vector2 midPoint = Vector2.Lerp(transform.position, targetCatchPos, 0.5f); // 중간점
        Vector2 curvePos1 = (midPoint - Vector2.Lerp(transform.position, midPoint, 0.5f)) + new Vector2(0, curveHeghit);
        Vector2 curvePos2 = (midPoint + Vector2.Lerp(targetCatchPos, midPoint, 0.5f)) + new Vector2(0, curveHeghit);

        //transform.position = BezierCurve_2(transform.position, curvePos, targetCatchPos, timerCurrent / timerMax);

        bezierPos[0] = curvePos1;
        bezierPos[1] = curvePos2;
        bezierPos[2] = targetCatchPos;

        transform.DOPath(bezierPos, 0.5f, PathType.CubicBezier, PathMode.Sidescroller2D, 30, Color.black);
    }

    Vector2 BezierCurve_2(Vector2 startPos, Vector2 curvePos, Vector2 endPos, float t)
    {
        Vector2 ab = Vector2.Lerp(startPos, curvePos, t);
        Vector2 cd = Vector2.Lerp(curvePos, endPos, t);

        return Vector2.Lerp(ab, cd, t);
    }

    public override void HitEvent()
    { 
        var ps = Instantiate(hitEffect);
        ps.transform.position = targetCatchPos;
        ps.Play();

        gameObject.SetActive(false);
    }

    //void OnDrawGizmos()
    //{
    //    for (float i = 0; i < 10; i++)
    //    {
    //        float value_Before = i / 10;
    //        Vector2 curvePos = transform.position + (distanceFromStartPos * -transform.right);
    //        Vector3 before = BezierCurve_2(transform.position, curvePos, targetCatchPos, value_Before);

    //        float value_After = (i + 1) / 10;
    //        Vector3 after = BezierCurve_2(transform.position, curvePos, targetCatchPos, value_After);

    //        Gizmos.color = Color.black;
    //        Gizmos.DrawLine(before, after);
    //    }

       
        
    //}

}

