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
            PopupText text = new PopupText();

            if (movedDist > rt.sizeDelta.y / 2)
            {
                if (Managers.Build.CanPlace(towerSO.placeTileType))
                {
                    if (Managers.Gold.GoldMinus(towerSO.PlaceCost))
                    {
                        Managers.Build.SpawnTower(towerSO, Managers.Build.Get2By2TilesCenter(Managers.Build.Get2By2Tiles()));

                        text.text = "설치 완료!";
                        Managers.UI.SummonPosText(towerImage.transform.position, text);
                    }
                    else
                    {
                        text.text = "타워 설치 비용이 부족합니다.";
                        Managers.UI.SummonPosText(towerImage.transform.position, text);
                    }
                }
                else
                {
                    text.text = "설치 불가능한 위치입니다.";
                    Managers.UI.SummonPosText(towerImage.transform.position, text);
                }
            }
            else
            {
                text.text = "설치 취소";
                text.maxSize = 20;
                Managers.UI.SummonPosText(towerImage.transform.position, text);
            }
        }

        OnDragEnd();
    }

    public void OnDragEnd()
    {
        towerImage.rectTransform.anchoredPosition = Vector3.zero; // 돌려보내기
        Managers.Build.ResetCheckedTiles(true);
        Managers.Build.movingImg = null;
        Managers.Build.map.tilemap_view_renderer.sortingOrder = -25; // 원래 -25
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
            Managers.Build.map.ShowPlaceableTiles(towerSO.placeTileType);
            Managers.Build.map.tilemap_view_renderer.sortingOrder = -4; // out Tilemap보다 1 높은 수.
        }
    }

    public bool CanDrag()
    {
        GameObject movingObj = Managers.Build.movingImg;

        return movingObj == towerImage.gameObject || movingObj == null; // 내거랑 다르거나 옮기고 있는 오브젝트가 없으면
    }

    bool IsLeftBtn(PointerEventData e) => e.button == PointerEventData.InputButton.Left;

}
