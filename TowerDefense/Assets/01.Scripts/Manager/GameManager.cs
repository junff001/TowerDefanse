using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int Hp { get; set; } = 10;
    public int Gold { get; set; } = 200;

    public static Text hpText; // 오펜스 / 디펜스 상태에 따라서 참조값이 다르다.

    public Transform waypointsParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    public Sprite waitSprite;

    public Dictionary<MonsterType, Sprite> enemySpriteDic = new Dictionary<MonsterType, Sprite>();
    public Sprite[] enemySprites; // 스프라이트 이름을 MonsterType에 써둔 enum명으로 해줘야 오류가 안생겨용
    
    //몇 스테이지 인가
    public int stageNum = 0;
    //1: 쉬움      2: 보통     3: 어려움
    public int stageLevel = 0;
    private void Awake()
    {
        SetWaypoints(waypointsParent);
        hpText = WaveManager.Instance.defenseHpText;
        InvadeManager.Instance.UpdateTexts();
        UpdateHPText();
        SetEnemySpriteDic();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
        Time.timeScale = 1;
    }

    public void SetEnemySpriteDic() 
    {
        enemySpriteDic.Add(MonsterType.None, waitSprite);
        foreach (var item in enemySprites)
        {
            enemySpriteDic.Add((MonsterType)Enum.Parse(typeof(MonsterType), item.name), item);
        }
    }

    public Sprite GetActBtnSprite(MonsterType monsterType) => enemySpriteDic[monsterType];

    public void UpdateHPText()
    {
        hpText.text = Hp.ToString();
    }

    public void SetWaypoints(Transform waypointParent)
    {
        waypointParent.GetComponentsInChildren(wayPoints);
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

