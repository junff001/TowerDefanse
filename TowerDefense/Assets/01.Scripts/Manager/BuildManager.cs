using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    private TileType curTileType = TileType.None;                  // 마우스에 위치한 현재 타일
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    private Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치

    public Map map;

    [SerializeField] private Tower towerBase;

    // 마우스 위치에 있는 타일
    public void SetCurTileType()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     // 마우스 월드 위치 
        tilePos = map.tilemap.WorldToCell(pos);                                    // 마우스 위치에 위치한 타일을 Vector3Int 로 변환
        tilePos -= new Vector3(                                                                          // 타일 자체를 받아와야 하고, 
        curTileType = map.mapTileTypeArray[tilePos.x, tilePos.y];
    }

    //색 바꾸는거랑 설치랑 같이 있었는데 if(GetMouseButton()) 밖에 있으면 좋아

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

    public bool CheckAroundTile() // 2x2 타일 검사
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 dir = map.tilemap.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0)) - map.tilemap.CellToWorld(tilePos); 

        int x = tilePos.x;
        int y = tilePos.y;

        Vector2Int upLeft =         new Vector2Int(x - 1, y + 1 );
        Vector2Int up =             new Vector2Int(x    , y + 1 );
        Vector2Int upRight =        new Vector2Int(x + 1, y + 1 );
        Vector2Int left =           new Vector2Int(x - 1, y     );
        Vector2Int curPos =         new Vector2Int(x    , y     );
        Vector2Int right =          new Vector2Int(x + 1, y     );
        Vector2Int downLeft =       new Vector2Int(x - 1, y - 1 );
        Vector2Int down =           new Vector2Int(x    , y - 1 );
        Vector2Int downRight =      new Vector2Int(x + 1, y - 1 );

        if (dir.x > 0 && dir.y > 0) // 1사분면
        {
            if (CanPlace(new Vector2Int[4] { curPos, up, upRight, right }))
            {
                return true;
            }
        }
        else if(dir.x < 0 && dir.y > 0)// 2사분면
        {
            if (CanPlace(new Vector2Int[4] { curPos, up, upLeft, left }))
            {
                return true;
            }
        }
        else if(dir.x < 0 && dir.y < 0)// 3사분면
        {
            if (CanPlace(new Vector2Int[4] { left,downLeft,down,curPos }))
            {
                return true;
            }
        }
        else if (dir.x > 0 && dir.y < 0) // 4사분면
        {
            if (CanPlace(new Vector2Int[4] { right,downRight,down,curPos }))
            {
                return true;
            }
        }
        else
        {
            Debug.Log("0일 수가 있군요?");
        }

        return false;

        bool CanPlace(Vector2Int[] checkPos)
        {
            bool canPlace = true;

            foreach (var pos in checkPos)
            {
                if (pos.x < 0 || pos.y < 0) canPlace = false; // -1이면 컷.
                if (pos.x < map.width || pos.y < map.height) canPlace = false; // -1이면 컷.

                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Place)
                {
                    canPlace = false;
                }
            }

            return canPlace;

        }
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(TowerSO towerSO)
    {
        map.mapTileTypeArray[tilePos.x, tilePos.y] = TileType.Tower;
        Tower newTower = Instantiate(towerBase);
        newTower.InitTowerData(towerSO);

        newTower.transform.position = map.tilemap.CellToWorld(tilePos);
    }

    // 다른 타일로 이동할 때, 전에 있던 곳 타일 빨간 색으로
    void SetBeforeTile()
    {
        if(beforeTilePos != null) // 처음은 널이니까
            map.tilemap.SetColor(beforeTilePos, Color.red); // 전 위치 색 초기화

        beforeTilePos = tilePos; // 내가 현재 있는 곳은 다음에 지워야할 위치임.
    }
}
