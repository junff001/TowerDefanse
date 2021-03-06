using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TowerInfo : MonoBehaviour
{
    private CanvasGroup canvasGroup;
    private Image image;

    [SerializeField] private CanvasGroup pageDefault;

    [SerializeField] private Button btnSale;
    [SerializeField] private Button btnLevelUp;
    [SerializeField] private Button btnInfo;
    [SerializeField] private Button btnCancel;

    private Tower currentSelectedTower;
    private Tower CurrentSelectedTower
    {
        get
        {
            return currentSelectedTower;
        }

        set
        {
            if (currentSelectedTower != null)
            {
                currentSelectedTower.ResetSortOrder();
            }

            value?.SetSortOrder("UI", 10);

            currentSelectedTower = value;
        }
    }
    private CanvasGroup beforePage = null;
    private CanvasGroup currentPage = null;

    private bool isOpenedInfo = false;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        btnCancel.onClick.AddListener(CallCancelBtnOnClicked);

        btnSale.onClick.AddListener(CallSaleBtnOnClicked);
        btnLevelUp.onClick.AddListener(CallUpgradeBtnOnClicked);
        btnInfo.onClick.AddListener(CallInfoBtnOnClicked);
    }

    public void OpenInfo(Tower tower)
    {
        if (tower == CurrentSelectedTower) return;

        image.SetNativeSize();

        if (!isOpenedInfo)
        {
            isOpenedInfo = true;
            Managers.UI.UIFade(canvasGroup, true);
        }

        OpenPage(pageDefault);
        BtnAnim(btnCancel, true);

        image.rectTransform.sizeDelta *= tower.TowerData.AttackRange;
        transform.position = tower.transform.position + new Vector3(0, 1, 0); // 오프셋
        CurrentSelectedTower = tower;
    }

    public void CloseInfo()
    {
        if (null == currentPage) return;
        Managers.UI.UIFade(canvasGroup, false);
        CanvasBtnsAnim(currentPage, false);
        isOpenedInfo = false;
        beforePage = null;
        currentPage = null;
        CurrentSelectedTower = null;
    }

    private void OpenPage(CanvasGroup page)
    {
        beforePage = currentPage;
        currentPage = page;

        CanvasBtnsAnim(currentPage, true);
        if (beforePage != currentPage)
        {
            if (beforePage != null)
            {
                CanvasBtnsAnim(beforePage, false);
            }
        }
    }

    private void CallSaleBtnOnClicked()
    {
        if(CurrentSelectedTower != null)
        {
            int index = Managers.Build.spawnedTowers.FindIndex(x => x == CurrentSelectedTower);

            Managers.Build.SetTowerGrid(CurrentSelectedTower, CurrentSelectedTower.myCheckedPos, false);

            float returnGold = CurrentSelectedTower.TowerData.PlaceCost * (Managers.Game.GetCoefficient().coefTowerPrice / 100);

            Managers.Gold.GoldPlus(returnGold);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(CurrentSelectedTower.transform.position);

            PopupText text = new PopupText($"타워 판매!\n{returnGold} Gold를 획득했습니다.");
            Managers.UI.SummonRectText(screenPos, text);

            Destroy(CurrentSelectedTower.gameObject);
            Managers.Build.spawnedTowers.RemoveAt(index);

            CloseInfo();
        }
    }

    private void CallUpgradeBtnOnClicked()
    {

    }

    private void CallInfoBtnOnClicked()
    {

    }

    private void CallCancelBtnOnClicked()
    {
        if(currentPage == pageDefault)
        {
            CloseInfo();
        }
    }

    private void CanvasBtnsAnim(CanvasGroup group, bool fade)
    {
        Button[] btns = group.GetComponentsInChildren<Button>();

        group.interactable = fade;
        group.blocksRaycasts = fade;
        group.DOFade(fade ? 1 : 0, 0.3f);

        foreach (Button btn in btns)
        {
            BtnAnim(btn, fade);
        }
    }

    private void BtnAnim(Button btn, bool fade)
    {
        if (fade)
        {
            btn.interactable = false;

            btn.targetGraphic.rectTransform.anchoredPosition = new Vector2(0, -150);
            btn.targetGraphic.rectTransform.DOAnchorPosY(0, 0.5f).OnComplete(() =>
            {
                btn.interactable = true;
            });
        }
        else
        {
            btn.interactable = false;

            btn.targetGraphic.rectTransform.anchoredPosition = new Vector2(0, 0);
            btn.targetGraphic.rectTransform.DOAnchorPosY(-150, 0.5f);
        }
    }
}
