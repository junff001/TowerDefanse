using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TowerPanel : MonoBehaviour, IEndDragHandler, IDragHandler
{
    [SerializeField] private Image towerImage = null;
    [SerializeField] private Text towerCostText = null;

    public TowerSO towerSO; // 이친구는 나중에 덱빌딩할 때 넣어줘

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        towerImage.sprite = towerSO.towerSprite;
        towerCostText.text = towerSO.PlaceCost.ToString();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (BuildManager.Instance.CanPlace(towerSO.placeTileType))
        {
            if (GameManager.Instance.Gold >= 100)
            {
                BuildManager.Instance.SpawnTower(towerSO);

                UIManager.SummonText(towerImage.transform.position, "설치 완료!", 30);
            }
            else
            {
                UIManager.SummonText(towerImage.transform.position, "타워 설치 비용이 부족합니다.", 30);
            }
        }
        else
        {
            UIManager.SummonText(towerImage.transform.position, "설치 불가능한 위치입니다.", 30);
        }

        towerImage.rectTransform.anchoredPosition = Vector3.zero; // 돌려보내기
        BuildManager.Instance.ResetCheckedTiles();
    }

    public void OnDrag(PointerEventData eventData)
    {
        towerImage.transform.position = Input.mousePosition;
        BuildManager.Instance.SetTilesColor(towerSO.placeTileType);
    }
}
