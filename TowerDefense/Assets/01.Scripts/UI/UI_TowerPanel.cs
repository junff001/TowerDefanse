using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UI_TowerPanel : MonoBehaviour, IEndDragHandler, IDragHandler, IBeginDragHandler
{
    private GameObject fakeTower;  // 건설 실루엣에 사용될 껍데기만 갖춘 타워이다.
    [SerializeField] private TextMeshProUGUI towerCostText = null;

    public TowerSO towerSO; // 이친구는 나중에 덱빌딩할 때 넣어줘
    private RectTransform rt;
    private RectTransform rangeObj;
    Vector3 rangeObjPlusPos = new Vector3(0, 0.5f, 0);

    void Start()
    {
        towerCostText.text = towerSO.PlaceCost.ToString();
        rt = GetComponent<RectTransform>();
        rangeObj = Managers.Build.rangeObj;
        SetFakeTower();
    }

    // InitTowerData 가 안된 타워
    void SetFakeTower()
    {
        Tower tower = Instantiate(Managers.Build.towerBase, transform);
        CoreBase core = null;

        if (towerSO.hasTower)
        {
            core = Managers.Build.MakeNewCore(towerSO, tower);
        }
        else
        {
            core = Managers.Build.MakeNoTowerCore(towerSO, tower);
        }

        tower.enabled = false;
        core.enabled = false;

        SpriteRenderer[] renderers = tower.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer item in renderers)
        {
            item.color = new Color(133 / 255f, 215 / 255f, 255 / 255f, 149 / 255f);
        }

        fakeTower = tower.gameObject;
        fakeTower.GetComponent<Tower>().SetSortOrder("UI", 10);
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
                        Managers.Build.SpawnTower(towerSO, Managers.Build.Get2x2TilesCenter(Managers.Build.Get2x2Tiles()));

                        text.text = "설치 완료!";
                        Managers.UI.SummonPosText(fakeTower.transform.position, text, true);
                    }
                    else
                    {
                        text.text = "타워 설치 비용이 부족합니다.";
                        Managers.UI.SummonPosText(fakeTower.transform.position, text, true);
                    }
                }
                else
                {
                    text.text = "설치 불가능한 위치입니다.";
                    Managers.UI.SummonPosText(fakeTower.transform.position, text, true);
                }
            }
            else
            {
                text.text = "설치 취소";
                text.maxSize = 20;
                Managers.UI.SummonPosText(fakeTower.transform.position, text, true);
            }
        }

        OnDragEnd();
    }

    public void OnDragEnd()
    {
        fakeTower.SetActive(false);

        fakeTower.transform.position = Vector3.zero; // 돌려보내기
        Managers.Build.ResetCheckedTiles(true);
        Managers.Build.movingObj = null;
        rangeObj.gameObject.SetActive(false);
        rangeObj.transform.localScale = Vector3.one;
        Managers.Build.map.tilemap_view_renderer.sortingOrder = -25; // 원래 -25
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (IsLeftBtn(eventData) && CanDrag())
        {
            Vector3 pos = Managers.Build.Get2x2TilesCenter(Managers.Build.Get2x2Tiles());
            
            rangeObj.transform.position = Camera.main.WorldToScreenPoint(pos + rangeObjPlusPos);
            fakeTower.transform.position = pos;
            Managers.Build.SetTilesColor(towerSO.placeTileType);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if(IsLeftBtn(eventData) && CanDrag())
        {
            fakeTower.SetActive(true);
            rangeObj.gameObject.SetActive(true);
            rangeObj.transform.localScale *= towerSO.AttackRange;
            Managers.Build.movingObj = fakeTower;
            Managers.Build.map.ShowPlaceableTiles(towerSO.placeTileType);
            Managers.Build.map.tilemap_view_renderer.sortingOrder = -4; // out Tilemap보다 1 높은 수.
        }
    }

    public bool CanDrag()
    {
        return (Managers.Build.movingObj == fakeTower || Managers.Build.movingObj == null) 
            && Managers.Wave.GameMode == Define.GameMode.DEFENSE; 
    }

    bool IsLeftBtn(PointerEventData e) => e.button == PointerEventData.InputButton.Left;

}
