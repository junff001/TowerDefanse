using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int Hp { get; set; } = 50;
    public int Gold { get; set; } = 5000;

    public Transform waypointParent;
    public List<GameObject> wayPoints = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    public void OnEnemyArrivedLastWaypoint(EnemyBase enemy)
    {
        Hp--;

        if (Hp < 0)
        {
            Hp = 0;
        }

        UIManager.Instance.UpdateHPText();

        Destroy(enemy.gameObject);
    }
}
