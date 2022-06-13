using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager
{
    private GameObject textPrefab = null;
    private Transform txtTrans = null;

    private UI_CheatCode cheatCodeGroup = null;

    public void Init()
    {
        GameObject uiManager = Resources.Load<GameObject>("UI/@UI");
        uiManager = Object.Instantiate(uiManager, null);
        MonoBehaviour.DontDestroyOnLoad(uiManager);

        textPrefab = Resources.Load<GameObject>("UI/PopupText");

        Transform canvas = uiManager.transform.Find("Canvas/TipTextBox");
        txtTrans = canvas;

        Transform cheatCode = uiManager.transform.Find("Canvas/CheatCode");
        cheatCodeGroup = cheatCode.GetComponent<UI_CheatCode>();
    }

    public void Update()
    {
        if (!cheatCodeGroup.isShow)
        {
            if (Input.GetKeyDown(KeyCode.Slash))
            {
                cheatCodeGroup.Show(true);
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                cheatCodeGroup.ResetWindow();
                cheatCodeGroup.Show(false);
            }

            if(Input.GetKeyDown(KeyCode.Return))
            {
                cheatCodeGroup.CheckCommand();
            }
        }
    }

    public void UIFade(CanvasGroup group, bool fade, float duration, bool setUpdate, UnityAction callback = null)
    {
        if (fade)
        {
            group.DOFade(1, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                group.interactable = true;
                group.blocksRaycasts = true;

                if (callback != null)
                    callback.Invoke();
            });
        }
        else
        {
            group.interactable = false;
            group.blocksRaycasts = false;

            group.DOFade(0, duration).SetUpdate(setUpdate).OnComplete(() =>
            {
                if (callback != null)
                    callback.Invoke();
            });
        }
    }

    public void UIFade(CanvasGroup group, bool fade)
    {
        group.alpha = fade ? 1 : 0;
        group.blocksRaycasts = fade;
        group.interactable = fade;
    }

    public void SummonText(Vector2 pos, string text, int maxSize, UnityAction callback = null)
    {
        Text textObj = Object.Instantiate(textPrefab, txtTrans).GetComponent<Text>();
        textObj.rectTransform.anchoredPosition = pos;
        textObj.text = text;
        textObj.resizeTextMaxSize = maxSize;

        textObj.rectTransform.DOAnchorPosY(100, 1.5f).SetRelative();
        textObj.DOFade(0, 3f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            if (callback != null)
                callback.Invoke();
            Object.Destroy(textObj);
        });
    }

    public void SummonText(Vector2 pos, string text, float time, int maxSize, UnityAction callback = null)
    {
        Text textObj = Object.Instantiate(textPrefab, txtTrans).GetComponent<Text>();
        textObj.rectTransform.anchoredPosition = pos;
        textObj.text = text;
        textObj.resizeTextMaxSize = maxSize;

        textObj.rectTransform.DOAnchorPosY(100, time / 2).SetRelative();
        textObj.DOFade(0, time).SetEase(Ease.InQuart).OnComplete(() =>
        {
            if (callback != null)
                callback.Invoke();
            Object.Destroy(textObj);
        });
    }

    public void SummonText(Vector2 pos, Vector2 dir, string text, float time, int maxSize, UnityAction callback = null)
    {
        Text textObj = Object.Instantiate(textPrefab, txtTrans).GetComponent<Text>();
        textObj.rectTransform.anchoredPosition = pos;
        textObj.text = text;
        textObj.resizeTextMaxSize = maxSize;

        textObj.rectTransform.DOAnchorPos(dir, time / 2).SetRelative();
        textObj.DOFade(0, time).SetEase(Ease.InQuart).OnComplete(() =>
        {
            if (callback != null)
                callback.Invoke();
            Object.Destroy(textObj);
        });
    }
}
