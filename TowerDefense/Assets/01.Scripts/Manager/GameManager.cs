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
    public RectTransform hpPopupTrans;

    public bool isAnyActing = false;

    public Transform waypointsParent { get; set; }

    [HideInInspector]
    public List<Transform> wayPoints = new List<Transform>();
    [HideInInspector]
    public List<IndexWayPointList> pointLists = new List<IndexWayPointList>();

    public Dictionary<Define.MonsterType, EnemySO> enemySoDic = new Dictionary<Define.MonsterType, EnemySO>();
    public EnemySO[] enemySOs;

    public GameObject clearUI;
    public GameObject gameOverUI;
    public UI_TowerInfo towerInfoUI = null;

    //1: 쉬움      2: 보통     3: 어려움
    public static int stageLevel = 0;

    private void Awake()
    {
        SetEnemySoDic();
    }

    private void Start()
    {
        SetWaypoints(waypointsParent);

        hpText = Managers.Wave.defenseHpText;
        Managers.Invade.UpdateTexts();
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

    public void SetEnemySoDic() 
    {
        foreach (var item in enemySOs)
        {
            enemySoDic.Add(item.MonsterType, item);
        }
    }

    public EnemySO GetActBtnSprite(Define.MonsterType monsterType) => enemySoDic[monsterType];

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

