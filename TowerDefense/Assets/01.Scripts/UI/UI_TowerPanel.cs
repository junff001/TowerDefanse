using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UI_TowerPanel : MonoBehaviour, IEndDragHandler, IDragHandler, IPointerDownHandler
{
    private Button myButton;

    [SerializeField] private Image towerImage = null;
    [SerializeField] private Text towerCostText = null;

    public TowerSO towerSO; // 이친구는 나중에 덱빌딩할 때 넣어줘

    private void Awake()
    {
        myButton = GetComponent<Button>();
    }

    public void BtnInit(Sprite towerSprite, int towerCost)
    {
        towerImage.sprite = towerSprite;
        towerCostText.text = towerCost.ToString();
    }



    public void OnEndDrag(PointerEventData eventData)
    {
        if(BuildManager.Instance.CheckAroundTile()) // 설치 가능 여부 전체 판단..
        {
            Debug.Log("가능");
            //BuildManager.Instance.SpawnTower(towerSO);
        }
        else
        {
            Debug.Log("불가능");
        }

        towerImage.transform.position = Vector3.zero; // 돌려보내기
    }

    public void OnDrag(PointerEventData eventData)
    {
        towerImage.transform.position = Input.mousePosition;

        BuildManager.Instance.SetHoveredTileColor();
        BuildManager.Instance.SetCurTileType();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //여기서 buildManager 설치할 타워 데이터 바꿔주기
    }
}
