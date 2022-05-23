using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    private static UIManager Instance;

    public GameObject textPrefab = null;
    public Transform txtTrans = null;

    public static void Init()
    {
        if(!Instance)
        {
            GameObject uiManager = Resources.Load<GameObject>("UIManager");
            uiManager = Instantiate(uiManager, null);

            Instance = uiManager.GetComponent<UIManager>();
            DontDestroyOnLoad(Instance.gameObject);
        }
        else
        {
            Instance.gameObject.SetActive(true);
        }
    }

    public static void UIFade(CanvasGroup group, bool fade, float duration, bool setUpdate, UnityAction callback = null)
    {
        Init();

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

    public static void SummonText(Vector2 pos, string text, int maxSize = 75, UnityAction callback = null)
    {
        Init();

        Text textObj = Instantiate(Instance.textPrefab, pos, Quaternion.identity, Instance.txtTrans).GetComponent<Text>();
        textObj.text = text;
        textObj.resizeTextMaxSize = maxSize;

        textObj.transform.DOMoveY(textObj.transform.position.y + 2, 1.5f);
        textObj.DOFade(0, 3f).SetEase(Ease.InQuart).OnComplete(() =>
        {
            if (callback != null)
                callback.Invoke();
            Destroy(textObj);
        });
    }
}
