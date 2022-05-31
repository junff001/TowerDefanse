using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

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

    void Update()
    {
        
    }
}
