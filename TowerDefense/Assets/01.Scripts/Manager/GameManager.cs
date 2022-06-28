using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;

public class PercentByLevel
{
    public float PercentByEnemyHP;
    public int PercentByTowerAttackPower;
    public int PercentByTowerPrice;
}
public class DefenseLevel : PercentByLevel
{
    [Serializable] public class EasyLevel : DefenseLevel { }
    [Serializable] public class NormalLevel : DefenseLevel { }
    [Serializable] public class HardLevel : DefenseLevel { }
}
public class OffenseLevel : PercentByLevel
{
    [Serializable] public class EasyLevel : DefenseLevel { }
    [Serializable] public class NormalLevel : DefenseLevel { }
    [Serializable] public class HardLevel : DefenseLevel { }
}

public class GameManager : MonoBehaviour
{
    [Header("Multiplied Value by StageLevel")]
    public DefenseLevel.EasyLevel easyLevel_DEF = new DefenseLevel.EasyLevel();
    public DefenseLevel.NormalLevel normalLevel_DEF = new DefenseLevel.NormalLevel();
    public DefenseLevel.HardLevel hardLevel_DEF = new DefenseLevel.HardLevel();
    [Space(10)]
    public OffenseLevel.EasyLevel easyLevel_OFF = new OffenseLevel.EasyLevel();
    public OffenseLevel.NormalLevel normalLevel_OFF = new OffenseLevel.NormalLevel();
    public OffenseLevel.HardLevel hardLevel__OFF = new OffenseLevel.HardLevel();
   
    public Dictionary<GameLevel, float> pctByEnemyHP_Dict_DEF = new Dictionary<GameLevel, float>();
    public Dictionary<GameLevel, int> pctByTowerAttackPower_Dict_DEF = new Dictionary<GameLevel, int>();
    public Dictionary<GameLevel, int> pctByTowerPrice_Dict_DEF = new Dictionary<GameLevel, int>();
    public Dictionary<GameLevel, float> pctByEnemyHP_Dict_OFF = new Dictionary<GameLevel, float>();
    public Dictionary<GameLevel, int> pctByTowerAttackPower_Dict_OFF = new Dictionary<GameLevel, int>();
    public Dictionary<GameLevel, int> pctByTowerPrice_Dict_OFF = new Dictionary<GameLevel, int>();

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
    public static GameLevel StageLevel { get; set; } = 0;

    private void Start()
    {
        #region Add Percent Value
        pctByEnemyHP_Dict_DEF.Add(GameLevel.Easy, easyLevel_DEF.PercentByEnemyHP);
        pctByEnemyHP_Dict_DEF.Add(GameLevel.Normal, normalLevel_DEF.PercentByEnemyHP);
        pctByEnemyHP_Dict_DEF.Add(GameLevel.Hard, hardLevel_DEF.PercentByEnemyHP);

        pctByTowerAttackPower_Dict_DEF.Add(GameLevel.Easy, easyLevel_DEF.PercentByTowerAttackPower);
        pctByTowerAttackPower_Dict_DEF.Add(GameLevel.Normal, normalLevel_DEF.PercentByTowerAttackPower);
        pctByTowerAttackPower_Dict_DEF.Add(GameLevel.Hard, hardLevel_DEF.PercentByTowerAttackPower);

        pctByTowerPrice_Dict_DEF.Add(GameLevel.Easy, easyLevel_DEF.PercentByTowerPrice);
        pctByTowerPrice_Dict_DEF.Add(GameLevel.Normal, normalLevel_DEF.PercentByTowerPrice);
        pctByTowerPrice_Dict_DEF.Add(GameLevel.Hard, hardLevel_DEF.PercentByTowerPrice);
        #endregion

        SetWaypoints(waypointsParent);

        hpText = Managers.Wave.defenseHpText;
        UpdateHPText();
    }

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

    public void OnEnemyArrivedLastWaypoint(EnemyBase enemy)
    {
        Hp--;

        PopupText text = new PopupText($"-1");
        text.textColor = Color.red;
        text.dir = new Vector2(0, -50);
        text.maxSize = 35;
        text.duration = 1;

        Managers.UI.SummonPosText(hpPopupTrans.transform.position, text);

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

