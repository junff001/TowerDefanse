using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class UI_TitleManager : MonoBehaviour
{
    public List<RectTransform>  statgePanel = new List<RectTransform>();

    public GameObject selectPanel;
    public GameObject lobyPanel;
    public GameObject topPanel;

    public int num = 0;
    public RectTransform mid;
    
    public Button nextBtn;
    public Button backBtn;
    public Button selectBtn;
    public Button exitBtn;
    bool isClick = false;

    void Start()
    {
        nextBtn.onClick.AddListener(() =>
        {
            NextSlide();
        });
        backBtn.onClick.AddListener(() =>
        {
            BackSlide();
        });
        selectBtn.onClick.AddListener(() =>
        {
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

    //앞으로 넘어가는 화면
    private void NextSlide()
    {
        if (!isClick)
        {
            statgePanel[num].DOMoveX(Screen.width * -2, 0.5f);
            if (num >= 3)
            {
                statgePanel[0].anchoredPosition = new Vector3(Screen.width * 2, 0);
                statgePanel[0].DOMoveX(mid.anchoredPosition.x, 0.5f);
                num = 0;
            }
            else
            {
                statgePanel[num + 1].anchoredPosition = new Vector3(Screen.width * 2, 0);
                statgePanel[num + 1].DOMoveX(mid.anchoredPosition.x, 0.5f);
                num++;
            }
            StartCoroutine("ClickDelay");
        }
    }

    //뒤으로 넘어가는 화면
    private void BackSlide()
    {
        if (!isClick)
        {
            statgePanel[num].DOMoveX(Screen.width * 2, 0.5f);
            if (num <= 0)
            {
                statgePanel[3].anchoredPosition = new Vector3(Screen.width * -2, 0);
                statgePanel[3].DOMoveX(mid.anchoredPosition.x, 0.5f);
                num = 3;
            }
            else
            {
                statgePanel[num - 1].anchoredPosition = new Vector3(Screen.width * -2, 0);
                statgePanel[num - 1].DOMoveX(mid.anchoredPosition.x, 0.5f);
                num--;
            }

            StartCoroutine(ClickDelay());
        }
    }

    IEnumerator ClickDelay()
    {
        isClick = true;
        yield return new WaitForSeconds(0.3f);
        isClick = false;
    }

    public void ChooseStage(int stage)
    {
        Debug.Log($"Target Stage : {stage}");
        Managers.Stage.SetTargetStage(stage - 1);
    }
}
