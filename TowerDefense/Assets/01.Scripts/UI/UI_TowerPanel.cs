using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TowerPanel : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    [SerializeField] private Image towerImage = null;
    [SerializeField] private Text towerCostText = null;

    public TowerSO towerSO; // 이친구는 나중에 덱빌딩할 때 넣어줘
    private RectTransform rt;


    private void Start()
    {
        Init();
        rt = this.GetComponent<RectTransform>();
    }

    public void Init()
    {
        towerImage.sprite = towerSO.towerSprite;
        towerCostText.text = towerSO.PlaceCost.ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (IsLeftBtn(eventData) && CanDrag())
        {
            float movedDist = Vector3.Distance(rt.anchoredPosition, eventData.position);
            if (movedDist > rt.sizeDelta.y / 2)
            {
                if (Managers.Build.CanPlace(towerSO.placeTileType))
                {
                    if (Managers.Gold.GoldMinus(towerSO.PlaceCost))
                    {
                        Managers.Build.SpawnTower(towerSO, Managers.Build.Get2By2TilesCenter(Managers.Build.Get2By2Tiles()));

                        Managers.UI.SummonText(towerImage.transform.position, "설치 완료!", 30);
                    }
                    else
                    {
                        Managers.UI.SummonText(towerImage.transform.position, "타워 설치 비용이 부족합니다.", 30);
                    }
                }
                else
                {
                    Managers.UI.SummonText(towerImage.transform.position, "설치 불가능한 위치입니다.", 30);
                }
            }
            else
            {
                Managers.UI.SummonText(towerImage.transform.position, "설치 취소", 20);
            }
        }

        OnDragEnd();
    }

    public void OnDragEnd()
    {
        towerImage.rectTransform.anchoredPosition = Vector3.zero; // 돌려보내기
        Managers.Build.ResetCheckedTiles();
        Managers.Build.movingImg = null;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsLeftBtn(eventData) && CanDrag())
        {
            towerImage.transform.position = Input.mousePosition;
            Managers.Build.SetTilesColor(towerSO.placeTileType);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(IsLeftBtn(eventData) && CanDrag())
        {
            Managers.Build.movingImg = towerImage.gameObject;
        }
    }

    public bool CanDrag()
    {
        GameObject movingObj = Managers.Build.movingImg;

        return movingObj == towerImage.gameObject || movingObj == null; // 내거랑 다르거나 옮기고 있는 오브젝트가 없으면
    }

    bool IsLeftBtn(PointerEventData e) => e.button == PointerEventData.InputButton.Left;

}
