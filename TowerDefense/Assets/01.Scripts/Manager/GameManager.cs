using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int Hp { get; set; } = 10;
    public int maxHp { get; set; } = 10;

    public static Text hpText; // 오펜스 / 디펜스 상태에 따라서 참조값이 다르다.
    public bool isAnyActing = false;

    public Transform waypointsParent;
    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();

    public Sprite waitSprite;

    public Dictionary<Define.MonsterType, Sprite> enemySpriteDic = new Dictionary<Define.MonsterType, Sprite>();
    public Sprite[] enemySprites; // 스프라이트 이름을 MonsterType에 써둔 enum명으로 해줘야 오류가 안생겨용

    public GameObject clearUI;
    public GameObject gameOverUI;

    //1: 쉬움      2: 보통     3: 어려움
    public static int stageLevel = 0;

    private void Awake()
    {
        SetEnemySpriteDic();
    }

    private void Start()
    {
        SetWaypoints(waypointsParent);

        hpText = Managers.Wave.defenseHpText;
        Managers.Invade.UpdateTexts();
        UpdateHPText();
    }

    public void LoadScene(string sceneName = null)
    {
        Time.timeScale = 1;

        if(sceneName == null)
        {
            sceneName = "SampleScene";
        }
        SceneManager.LoadScene(sceneName);
    }

    public void LoadCurScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void SetEnemySpriteDic() 
    {
        enemySpriteDic.Add(Define.MonsterType.None, waitSprite);
        foreach (var item in enemySprites)
        {
            enemySpriteDic.Add((Define.MonsterType)Enum.Parse(typeof(Define.MonsterType), item.name), item);
        }
    }

    public Sprite GetActBtnSprite(Define.MonsterType monsterType) => enemySpriteDic[monsterType];

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

        if (Hp <= 0)
        {
            Hp = 0;
            gameOverUI.gameObject.SetActive(true);
            Time.timeScale = 0;
        }
        UpdateHPText();
        Managers.Wave.aliveEnemies.Remove(enemy);
        Managers.Wave.CheckWaveEnd();
        Destroy(enemy.gameObject);
    }
}

