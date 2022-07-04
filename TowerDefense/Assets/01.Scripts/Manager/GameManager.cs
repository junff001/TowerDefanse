using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;

public class CoefByDifficulty // Coefficient = 계수 = coef
{
    public int coefEnemyHP;
    public int coefTowerOffensePower;
    public int coefTowerPrice;
    public int coefEnemyOffensePower;
    public int coefTowerHP;

    public CoefByDifficulty(int coefEnemyHP, int coefTowerOffensePower, int coefTowerPrice, int coefEnemyOffensePower, int coefTowerHP)
    {
        this.coefEnemyHP            = coefEnemyHP;
        this.coefTowerOffensePower  = coefTowerOffensePower;
        this.coefTowerPrice         = coefTowerPrice;
        this.coefEnemyOffensePower  = coefEnemyOffensePower;
        this.coefTowerHP            = coefTowerHP;
    }
}

public class GameManager : MonoBehaviour
{
    [Header("Dict by GameDifficulty")]
    public Dictionary<GameDifficulty, CoefByDifficulty> coefDict;

    public int Hp { get; set; } = 10;
    public int maxHp { get; set; } = 10;

    public static TextMeshProUGUI hpText; // 오펜스 / 디펜스 상태에 따라서 참조값이 다르다.
    public RectTransform hpPopupTrans;

    public bool isAnyActing = false;

    public Transform waypointsParent { get; set; }

    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();
    [HideInInspector]
    public List<IndexWayPointList> pointLists = new List<IndexWayPointList>();

    public GameObject clearUI;
    public GameObject gameOverUI;
    public UI_TowerInfo towerInfoUI = null;

    public Sprite waitSprite;

    //1: 쉬움      2: 보통     3: 어려움
    public static GameDifficulty GameDifficulty { get; set; } = GameDifficulty.Easy;

    private void Start()
    {
        coefDict = new Dictionary<GameDifficulty, CoefByDifficulty>()
        {
            { GameDifficulty.Easy, new CoefByDifficulty  (100, 100, 100, 100, 100) },
            { GameDifficulty.Normal, new CoefByDifficulty(100, 100, 100, 100, 100) },
            { GameDifficulty.Hard, new CoefByDifficulty  (100, 100, 100, 100, 100) },
        };
        SetWaypoints(waypointsParent);

        hpText = Managers.Wave.defenseHpText;
        UpdateHPText();
    }

    public CoefByDifficulty GetCoefficient() => coefDict[GameDifficulty];

    public void LoadScene()
    {
        Time.timeScale = 1;

        Managers.LoadScene.LoadScene("SampleScene");
    }

    public void LoadCurScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void LoadStageSelect()
    {
        Managers.LoadScene.LoadScene("TitleScene");
        Time.timeScale = 1;
    }

    public void UpdateHPText()
    {
        hpText.text = Hp.ToString();
    }

    public int GetWaypointCount(int listIndex)
    {
        return pointLists[listIndex].indexWayPoints.Count;
    }

    public void SetWaypoints(Transform waypointParent)
    {
        waypointParent.GetComponentsInChildren(wayPoints);
        wayPoints.RemoveAt(0);
    }

    public void OnEnemyArrivedLastWaypoint(Enemy enemy)
    {
        Hp--;

        PopupText text = new PopupText($"-1");
        text.textColor = Color.red;
        text.dir = new Vector2(0, -50);
        text.maxSize = 35;
        text.duration = 1;

        Managers.UI.SummonPosText(hpPopupTrans.transform.position, text, false);

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

    public void SetHP(int value)
    {
        Hp = value;
        UpdateHPText();
    }
}

