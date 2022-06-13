using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_TowerInfo : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    [SerializeField] private Button btnLevelUp;
    [SerializeField] private Button btnSale;
    [SerializeField] private Button btnCancel;

    private Tower currentSelectedTower;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        btnSale.onClick.AddListener(CallSaleBtnOnClicked);

        btnCancel.onClick.AddListener(() =>
        {
            Managers.UI.UIFade(canvasGroup, false);
        });


    }

    public void OpenInfo(Tower tower)
    {
        Managers.UI.UIFade(canvasGroup, true);
        BtnAnim(btnLevelUp, true);
        BtnAnim(btnSale, true);
        BtnAnim(btnCancel, true);

        transform.position = tower.transform.position + new Vector3(0, 1, 0); // 오프셋
        currentSelectedTower = tower;
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
            Managers.UI.SummonText(screenPos, $"타워 판매!\n{returnGold} Gold를 획득했습니다.", 30);

            Destroy(currentSelectedTower.gameObject);
            Managers.Build.spawnedTowers.RemoveAt(index);

            Managers.UI.UIFade(canvasGroup, false);
        }
    }

    private void CallCancelBtnOnClicked()
    {

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
            btn.targetGraphic.rectTransform.anchoredPosition = new Vector2(0, 0);
            btn.targetGraphic.rectTransform.DOAnchorPosY(-150, 0.5f);
        }
    }
}
