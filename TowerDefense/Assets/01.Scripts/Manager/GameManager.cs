using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : Singleton<GameManager>
{
    public int Hp { get; set; } = 10;
    public int Gold { get; set; } = 500;

    public static Text hpText; // 오펜스 / 디펜스 상태에 따라서 참조값이 다르다.

    public Transform waypointsParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    public Sprite[] enemySprites;
    public Sprite waitSprite;

    public Dictionary<string, Sprite> tileSpritesDic = new Dictionary<string, Sprite>();

    private void Awake()
    {
        SetWaypoints(waypointsParent);
        hpText = WaveManager.Instance.defenseHpText;
        InvadeManager.Instance.UpdateTexts();
        UpdateHPText();
    }

    private void Start()
    {
        // Test
        //RecordManager.Instance.StartRecord();

    }

    public Sprite GetActBtnSprite(MonsterType monsterType)
    {
        Sprite retSpr = null;
        switch (monsterType)
        {
            case MonsterType.Goblin:
                retSpr = enemySprites[0];
                break;
        }
        //여기까지 오면 Wait 추가 버튼임.
        if (retSpr == null) retSpr = waitSprite;
        return retSpr;
    }

    public void UpdateHPText()
    {
        hpText.text = Hp.ToString();
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
        UpdateHPText();
        WaveManager.Instance.aliveEnemies.Remove(enemy);
        WaveManager.Instance.CheckWaveEnd();
        Destroy(enemy.gameObject);
    }
}

