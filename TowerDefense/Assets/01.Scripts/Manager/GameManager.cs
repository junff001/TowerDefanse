using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int Hp { get; set; } = 50;
    public int Gold { get; set; } = 500;

    public Transform waypointsParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    public Sprite[] enemySprites;
    public Sprite waitSprite;

    public Dictionary<string, Sprite> tileSpritesDic = new Dictionary<string, Sprite>();

    private void Awake()
    {
        if(wayPoints.Count > 0)
        {
            SetWaypoints(waypointsParent);
        }
    }

    public Sprite GetActBtnSprite(MonsterType monsterType)
    {
        Sprite retSpr = null;
        switch(monsterType)
        {
            case MonsterType.Goblin:
                retSpr = enemySprites[0];
                break;
            case MonsterType.Ghost:
                retSpr = enemySprites[1];
                break;
            case MonsterType.Slime:
                retSpr = enemySprites[2];
                break;
            case MonsterType.IronBore:
                retSpr = enemySprites[3];
                break;
        }
        if (retSpr == null) retSpr = waitSprite;
        return retSpr;
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

