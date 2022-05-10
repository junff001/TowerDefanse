using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int Hp { get; set; } = 50;
    public int Gold { get; set; } = 500;
    public Vector2 mousePosition { get; set; } = Vector2.zero;

    public Transform waypointsParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    private void Awake()
    {
        if(wayPoints.Count > 0)
        {
            SetWaypoints(waypointsParent);
        }
    }

    public void SetWaypoints(Transform waypointParent)
    {
        waypointParent.GetComponentsInChildren<Transform>(wayPoints);
        wayPoints.RemoveAt(0);
    }

    public void OnEnemyArrivedLastWaypoint(EnemyBase enemy)
    {
        Hp--;

        if (Hp < 0)
        {
            Hp = 0;
        }

        UIManager.Instance.UpdateHPText();
        WaveManager.Instance.aliveEnemies.Remove(enemy);
        Destroy(enemy.gameObject);
    }
}

