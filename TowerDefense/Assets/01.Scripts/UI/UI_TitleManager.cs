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
    public GameObject difficultyPanel;

    public int num = 0;
    public TextMeshProUGUI diffcultyTex;
    public RectTransform mid;
    
    public Button nextBtn;
    public Button backBtn;
    public Button selectBtn;
    public Button exitBtn;
    bool isClick = false;

    void Start()
    {
        difficultyPanel.SetActive(true);

        Diffculty();

        // UI 이동
        difficultyPanel.transform.localScale = new Vector3(0, 0, 0);
        selectPanel.transform.position = new Vector3(mid.anchoredPosition.x, Screen.height * -2);
        topPanel.transform.position = new Vector3(mid.anchoredPosition.x, Screen.height * 2);

        nextBtn.onClick.AddListener(() =>
        {
            Diffculty();
            NextSlide();
        });
        backBtn.onClick.AddListener(() =>
        {
            Diffculty();
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

    public void Diffculty()
    {
        switch (num)
        {
            case 0 :
                diffcultyTex.text = "Easy";
                diffcultyTex.color = new Color(0, 255, 0);
                break;
            case 1:
                diffcultyTex.text = "Normal";
                diffcultyTex.color = new Color(255, 250, 0);
                break;
            case 2:
                diffcultyTex.text = "Hard";
                diffcultyTex.color = new Color(255, 0, 0);
                break;
            case 3:
                diffcultyTex.text = "Tutorial";
                diffcultyTex.color = new Color(150, 255, 255);
                break;
        }
    }

    IEnumerator ClickDelay()
    {
        Diffculty();
        isClick = true;
        yield return new WaitForSeconds(0.3f);
        isClick = false;
    }

    public void ChooseStage(int stage)
    {
        Debug.Log($"Target Stage : {stage}");
        difficultyPanel.transform.DOScale(1, 0.5f).SetEase(Ease.OutBounce);
        Managers.Stage.SetTargetStage(stage - 1);
    }
}
