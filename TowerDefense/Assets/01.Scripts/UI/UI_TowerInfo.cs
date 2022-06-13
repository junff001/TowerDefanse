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

    [SerializeField] private Button[] btnProperties; // �Ӽ� enum ������ �����Ұ�

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

        transform.position = tower.transform.position + new Vector3(0, 1, 0); // ������
        currentSelectedTower = tower;
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

            float returnGold = currentSelectedTower.TowerData.PlaceCost * 0.6f; // TO DO : ���߿� ���̵� ���� �޶����
            Managers.Gold.GoldPlus(returnGold);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(currentSelectedTower.transform.position);
            Managers.UI.SummonText(screenPos, $"Ÿ�� �Ǹ�!\n{returnGold} Gold�� ȹ���߽��ϴ�.", 30);

            Destroy(currentSelectedTower.gameObject);
            Managers.Build.spawnedTowers.RemoveAt(index);

            Managers.UI.UIFade(canvasGroup, false);
        }
    }

    private void CallPropertyBtnOnClicked(Define.PropertyType type)
    {
        if (currentSelectedTower != null)
        {
            currentSelectedTower.ChangeProperty(type);
        }
    }

    private void CallCancelBtnOnClicked()
    {
        if(currentPage == pageDefault)
        {
            Managers.UI.UIFade(canvasGroup, false);
            beforePage = null;
        }
        else if (currentPage == pageProperty)
        {
            OpenPage(pageDefault);
        }
    }

    private void CanvasBtnsAnim(CanvasGroup canvasgroup, bool fade)
    {
        Button[] btns = canvasgroup.GetComponentsInChildren<Button>();

        canvasGroup.interactable = fade;
        canvasGroup.blocksRaycasts = fade;
        canvasGroup.DOFade(fade ? 1 : 0, 0.3f);

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
