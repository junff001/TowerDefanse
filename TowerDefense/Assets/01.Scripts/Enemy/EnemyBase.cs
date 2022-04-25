using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D rigid { get; set; }

    [SerializeField] private float moveSpeed = 0f;
    private int currentWayPointIndex = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Move();
    }

    // WayPoint�� ���� �̵�
    void Move()
    {
        if (currentWayPointIndex >= GameManager.Instance.wayPoints.Count) return;

        Vector2 direction = GameManager.Instance.wayPoints[currentWayPointIndex].transform.position - transform.position;
        direction.Normalize();
        rigid.velocity = new Vector2(direction.x * moveSpeed, direction.y * moveSpeed);

        if (WayPointDistance())
        {
            NextPoint();
        }
    }

    bool WayPointDistance()
    {
        if (Vector2.Distance(GameManager.Instance.wayPoints[currentWayPointIndex].transform.position, transform.position) < 0.3f)
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
