using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BuildManager : Singleton<BuildManager>, IDragHandler, IEndDragHandler
{
    private TileType curTileType = TileType.None;                  // 마우스에 위치한 현재 타일
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    private Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치

    public Map map;
                 
    // 마우스 위치에 있는 타일
    void SetCurTileType()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     // 마우스 월드 위치 
        tilePos = map.tilemap.WorldToCell(pos);                                    // 마우스 위치에 위치한 타일을 Vector3Int 로 변환
        curTileType = map.mapTileTypeArray[tilePos.x, tilePos.y];
    }

    public void SetHoveredTileColor()
    {
        SetBeforeTile();
        if(curTileType == TileType.Place)
        {
            map.tilemap.SetColor(beforeTilePos, new Color(0,0,1,0.5f));
        }
        else
        {
            map.tilemap.SetColor(beforeTilePos, new Color(1, 0, 0, 0.5f));
        }
    }

    // 현재 타일이 뭔지 확인하는 함수
    public bool IsPlaceableTile()
    {
        if (curTileType == TileType.Place)
        {
            return true;
        }
        return false;
    }

    // 타워를 스폰하는 함수
    public void SpawnTower()
    {
        
    }

    // 다른 타일로 이동할 때, 전에 있던 곳 타일 빨간 색으로
    void SetBeforeTile()
    {
        if(beforeTilePos != null) // 처음은 널이니까
            map.tilemap.SetColor(beforeTilePos, Color.red); // 전 위치 색 초기화

        beforeTilePos = tilePos; // 내가 현재 있는 곳은 다음에 지워야할 위치임.
    }

    public void OnDrag(PointerEventData eventData)
    {
        SetCurTileType();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(IsPlaceableTile())
        {
            SpawnTower();
        }
    }
}
