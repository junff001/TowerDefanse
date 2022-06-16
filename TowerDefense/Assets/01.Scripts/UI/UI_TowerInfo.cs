using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TowerInfo : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] private CanvasGroup pageDefault;
    [SerializeField] private CanvasGroup pageProperty;

    [SerializeField] private Button btnLevelUp;
    [SerializeField] private Button btnSale;
    [SerializeField] private Button btnCancel;

    [SerializeField] private Button[] btnProperties; // 속성 enum 순으로 정렬할것

    private Tower currentSelectedTower;
    private CanvasGroup beforePage = null;
    private CanvasGroup currentPage = null;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        btnCancel.onClick.AddListener(CallCancelBtnOnClicked);

        btnSale.onClick.AddListener(CallSaleBtnOnClicked);
        btnLevelUp.onClick.AddListener(CallUpgradeBtnOnClicked);

        for (int i = 0; i < btnProperties.Length; i++)
        {
            Define.PropertyType type = (Define.PropertyType)(i + 1);
            btnProperties[i].onClick.AddListener(() => CallPropertyBtnOnClicked(type));
        }
    }

    public void OpenInfo(Tower tower)
    {
        Managers.UI.UIFade(canvasGroup, true);
        OpenPage(pageDefault);
        BtnAnim(btnCancel, true);

        transform.position = tower.transform.position + new Vector3(0, 1, 0); // 오프셋
        currentSelectedTower = tower;
    }

    public void CloseInfo()
    {
        if (null == currentPage) return;
        Managers.UI.UIFade(canvasGroup, false);
        CanvasBtnsAnim(currentPage, false);
        beforePage = null;
        currentPage = null;
    }

    private void OpenPage(CanvasGroup page)
    {
        beforePage = currentPage;
        currentPage = page;

        CanvasBtnsAnim(page, true);

        if (beforePage != null)
        {
            CanvasBtnsAnim(beforePage, false);
        }
    }

    private void CallUpgradeBtnOnClicked()
    {
        OpenPage(pageProperty);
    }

    private void CallSaleBtnOnClicked()
    {
        if(currentSelectedTower != null)
        {
            int index = Managers.Build.spawnedTowers.FindIndex(x => x == currentSelectedTower);

            Managers.Build.SetTowerGrid(currentSelectedTower, currentSelectedTower.myCheckedPos, false);

            RecordTowerSale recordSegment = new RecordTowerSale(index);
            Managers.Record.AddRecord(recordSegment);

            float returnGold = currentSelectedTower.TowerData.PlaceCost * 0.6f; // TO DO : 나중에 난이도 따라 달라야함
            Managers.Gold.GoldPlus(returnGold);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentSelectedTower.transform.position);

            PopupText text = new PopupText($"타워 판매!\n{returnGold} Gold를 획득했습니다.");
            Managers.UI.SummonRectText(screenPos, text);

            Destroy(currentSelectedTower.gameObject);
            Managers.Build.spawnedTowers.RemoveAt(index);

            Managers.UI.UIFade(canvasGroup, false);
        }
    }

    private void CallPropertyBtnOnClicked(Define.PropertyType type)
    {
        Debug.Log(type);
        if (currentSelectedTower != null)
        {
            //currentSelectedTower.ChangeProperty(type);
            CloseInfo();
        }
    }

    private void CallCancelBtnOnClicked()
    {
        if(currentPage == pageDefault)
        {
            CloseInfo();
        }
        else if (currentPage == pageProperty)
        {
            OpenPage(pageDefault);
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
