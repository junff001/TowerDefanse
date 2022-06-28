using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static Define;
using TMPro;

public class CoefByDifficulty // Coefficient = 계수 = coef
{
    public int coefEnemyHP;
    public int coefTowerAttackPower;
    public int coefTowerPrice;
    //public int 

}

public class GameDifficulty : CoefByDifficulty
{
    [Serializable] public class EasyLevel : GameDifficulty { }
    [Serializable] public class NormalLevel : GameDifficulty { }
    [Serializable] public class HardLevel : GameDifficulty { }
}

public class GameManager : MonoBehaviour
{
    [Header("Multiplied Value by StageLevel")]
    public GameDifficulty.EasyLevel easyLevel_DEF = new GameDifficulty.EasyLevel();
    public GameDifficulty.NormalLevel normalLevel_DEF = new GameDifficulty.NormalLevel();
    public GameDifficulty.HardLevel hardLevel_DEF = new GameDifficulty.HardLevel();
   

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

