using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class UI_TitleManager : MonoBehaviour
{
    public UI_WorldMap[] worldMaps;
    public RectTransform stageMapRect;
    private int scrollIndex = 0;

    public GameObject selectPanel;
    public GameObject lobyPanel;
    public GameObject topPanel;

    public RectTransform mid;
    
    public Button nextBtn;
    public Button backBtn;
    public Button selectBtn;
    public Button exitBtn;

    void Start()
    {
        nextBtn.onClick.AddListener(() =>
        {
            SetIndexMap(scrollIndex + 1);
        });

        backBtn.onClick.AddListener(() =>
        {
            SetIndexMap(scrollIndex - 1);
        });

        selectBtn.onClick.AddListener(() =>
        {
            if(scrollIndex == 0)
            {
                backBtn.gameObject.SetActive(false);
            }

            lobyPanel.GetComponent<RectTransform>().DOMoveY(Screen.height * -2, 0.5f);
            topPanel.GetComponent<RectTransform>().DOMoveY(mid.anchoredPosition.y, 0.4f).SetEase(Ease.OutBack).OnComplete(()=> {
                selectPanel.GetComponent<RectTransform>().DOMoveY(mid.anchoredPosition.y, 0.5f).SetEase(Ease.OutBack);
            });
        });
        exitBtn.onClick.AddListener(() =>
        {
            lobyPanel.GetComponent<RectTransform>().DOMoveY(mid.anchoredPosition.y, 0.5f).SetEase(Ease.OutBack);
            topPanel.GetComponent<RectTransform>().DOMoveY(Screen.height * 2, 0.4f);
            selectPanel.GetComponent<RectTransform>().DOMoveY(Screen.height * -2, 0.5f);
        });
    }

    private void SetIndexMap(int index)
    {
        scrollIndex = index;

        backBtn.gameObject.SetActive(scrollIndex > 0);
        nextBtn.gameObject.SetActive(scrollIndex < worldMaps.Length - 1);

        Slide(scrollIndex);
    }

    //앞으로 넘어가는 화면
    private void Slide(int index)
    {
        stageMapRect.DOAnchorPosX(-Screen.width * index, 0.5f);
    }
}
