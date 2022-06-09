using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UI_ChooseDiffculty : MonoBehaviour
{
    public Button exitBtn;
    public GameObject difficultyPanel;

    public Button eseyBtn;
    public Button nomalBtn;
    public Button hardBtn;

    void Start()
    {
        exitBtn.onClick.AddListener(() =>
        {
            difficultyPanel.transform.DOScale(0, 0.3f);
        });

        eseyBtn.onClick.AddListener(() =>
        {
            GoInGame(1);
        });
        nomalBtn.onClick.AddListener(() =>
        {
            GoInGame(2);
        });
        hardBtn.onClick.AddListener(() =>
        {
            GoInGame(3);
        });
    }

    public void GoInGame(int Level)
    {
        GameManager.stageLevel = 3;
        Managers.LoadScene.LoadScene("JuhyeongScene");
    }
}
