using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TowerInfo : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    public Button btnLevelUp;
    public Button btnSale;
    public Button btnCancel;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        btnCancel.onClick.AddListener(() =>
        {
            Managers.UI.UIFade(canvasGroup, false);
        });
    }

    [ContextMenu("test")]
    public void OpenInfo(/*Tower tower*/)
    {
        Managers.UI.UIFade(canvasGroup, true);
        BtnAnim(btnLevelUp.targetGraphic.rectTransform, true);
        BtnAnim(btnSale.targetGraphic.rectTransform, true);
        BtnAnim(btnCancel.targetGraphic.rectTransform, true);


    }

    private void BtnAnim(RectTransform rect, bool fade)
    {
        if (fade)
        {
            rect.anchoredPosition = new Vector2(0, -150);
            rect.DOAnchorPosY(0, 0.75f);
        }
        else
        {
            rect.anchoredPosition = new Vector2(0, 0);
            rect.DOAnchorPosY(-150, 0.75f);
        }
    }
}
