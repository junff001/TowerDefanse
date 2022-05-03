using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public int Hp { get; set; } = 50;
    public int Gold { get; set; } = 500;
    public Vector2 mousePosition { get; set; } = Vector2.zero;
    

    public Transform waypointParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    void Awake()
    {
        Instance = this;
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

    // 마우스 포지션 처리 함수
    public void MousePosition(InputAction.CallbackContext context)
    {
        mousePosition = context.ReadValue<Vector2>();
    }
}

