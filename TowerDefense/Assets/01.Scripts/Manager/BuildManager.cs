using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    public TileType curTileType = TileType.None;                  // 마우스에 위치한 현재 타일
    public Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    public Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치

    public Map map;
    [SerializeField] private Tower towerBase;

    // 마우스 위치에 있는 타일
    public void SetCurTileType()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     // 마우스 월드 위치 
        tilePos = map.tilemap.WorldToCell(pos);    // 마우스 위치에 위치한 타일을 Vector3Int 로 변환
        curTileType = map.mapTileTypeArray[tilePos.x, tilePos.y];
    }

    public void SetTilesColor()
    {
        foreach (var pos in checkedPos) // 처음에는 null이라서 실행 안할거임.
        {
            map.tilemap.SetColor(pos, Color.white);
        }
        //checkedPos = checkPos; // 내가 체크할 포지션들을 나중에 지워주야

        //foreach (var pos in checkPos)
        //{
        //    if (map.mapTileTypeArray[pos.x, pos.y] == TileType.Place)
        //    {
        //        map.tilemap.SetColor(pos, new Color(0, 0, 1, 0.5f)); // 아마 레드
        //    }
        //    else
        //    {
        //        map.tilemap.SetColor(pos, new Color(1, 0, 0, 0.5f)); // 아마 파랑
        //    }
        //}
    }

    public bool CheckAroundTile() // 2x2 타일 검사
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 dir = map.tilemap.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0)) - map.tilemap.CellToWorld(tilePos); 
        int x = tilePos.x, y = tilePos.y, z = tilePos.z;

        Vector3Int upLeft =         new Vector3Int(x - 1, y + 1 , z);
        Vector3Int up =             new Vector3Int(x    , y + 1 , z);
        Vector3Int upRight =        new Vector3Int(x + 1, y + 1 , z);
        Vector3Int left =           new Vector3Int(x - 1, y     , z);
        Vector3Int curPos =         new Vector3Int(x    , y     , z);
        Vector3Int right =          new Vector3Int(x + 1, y     , z);
        Vector3Int downLeft =       new Vector3Int(x - 1, y - 1 , z);
        Vector3Int down =           new Vector3Int(x    , y - 1 , z);
        Vector3Int downRight =      new Vector3Int(x + 1, y - 1 , z);

        if (dir.x > 0 && dir.y > 0) // 1사분면
        {
            if (CanPlace(new Vector3Int[4] { curPos, up, upRight, right }))
            {
                return true;
            }
        }
        else if(dir.x < 0 && dir.y > 0)// 2사분면
        {
            if (CanPlace(new Vector3Int[4] { curPos, up, upLeft, left }))
            {
                return true;
            }
        }
        else if(dir.x < 0 && dir.y < 0)// 3사분면
        {
            if (CanPlace(new Vector3Int[4] { left,downLeft,down,curPos }))
            {
                return true;
            }
        }
        else if (dir.x > 0 && dir.y < 0) // 4사분면
        {
            if (CanPlace(new Vector3Int[4] { right,downRight,down,curPos }))
            {
                return true;
            }
        }
        else
        {
            Debug.Log("0일 수가 있군요?");
        }

        return false;

        bool CanPlace(Vector3Int[] checkPos)
        {


            foreach (var pos in checkPos)
            {
                if (pos.x < 0 || pos.y < 0) return false;
                if (pos.x >= map.width -1  || pos.y >= map.height - 1) return false;
                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Place) return false;


            }

            //여기까지 왔으면 다 사용 가능한거임.
            return true;
        }
    }

    public Vector3Int[] checkedPos = null;

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
            map.tilemap.SetColor(beforeTilePos, Color.white); // 전 위치 색 초기화

        beforeTilePos = tilePos; // 내가 현재 있는 곳은 다음에 지워야할 위치임.
    }
}
