using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : Enemy
{
    public List<Transform> wayPoints = null;

    [SerializeField] private float moveSpeed = 0f;
    private int currentWayPointIndex = 0;

    void Update()
    {
        Move();
    }

    // WayPoint를 따라 이동
    void Move()
    {
        Vector2 direction = wayPoints[currentWayPointIndex].position - transform.position; 
        direction.Normalize();
        rigid.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        if (WayPointDistance())
        {
            NextPoint();
        }
    }

    bool WayPointDistance()
    {
        if (Vector2.Distance(wayPoints[currentWayPointIndex].position, transform.position) < 0.3f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void NextPoint()
    {
        currentWayPointIndex += 1;
    }
}
