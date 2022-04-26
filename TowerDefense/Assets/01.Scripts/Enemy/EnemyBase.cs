using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
    public Rigidbody2D rigid { get; set; }
    private HealthSystem healthSystem;

    [SerializeField] private float moveSpeed = 0f;
    private int currentWayPointIndex = 0;

    public float aliveTime = 0f;
    public float movedDistance = 0f;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();
    }

    void Update()
    {
        aliveTime += Time.deltaTime;
        movedDistance = aliveTime * moveSpeed;
        Move();
    }

    // WayPoint�� ���� �̵�
    void Move()
    {
        if (currentWayPointIndex == GameManager.Instance.wayPoints.Count)
        {
            GameManager.Instance.OnEnemyArrivedLastWaypoint(this);
            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, GameManager.Instance.wayPoints[currentWayPointIndex].transform.position,
            Time.deltaTime * moveSpeed);

        if (WayPointDistance())
        {
            NextPoint();
        }
    }

    bool WayPointDistance()
    {
        if (Vector2.Distance(GameManager.Instance.wayPoints[currentWayPointIndex].transform.position, transform.position) < 0.01f)
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

    public void WaveStatControl(int wave)
    {
        float value_f = (wave * Mathf.Pow(1.5f, 0)) * 100;
        int value = (int)value_f;

        healthSystem.SetHealthAmountMax(value, true); // 체력 조절
    }
}
