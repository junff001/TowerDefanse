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

    void Start()
    {
        exitBtn.onClick.AddListener(() =>
        {
            difficultyPanel.transform.DOScale(0, 0.3f);
        });
    }

    public void GoInGame(int Level, string sceneName)
    {
        GameManager.stageLevel = 3;
        Managers.LoadScene.LoadScene(sceneName);
    }
}
