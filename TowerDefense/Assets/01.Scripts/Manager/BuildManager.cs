using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치

    private Vector3Int[] checkedPos = null;

    private Vector3Int upLeft = Vector3Int.zero;
    private Vector3Int up = Vector3Int.zero;
    private Vector3Int upRight = Vector3Int.zero;
    private Vector3Int left = Vector3Int.zero;
    private Vector3Int curPos = Vector3Int.zero;
    private Vector3Int right = Vector3Int.zero;
    private Vector3Int downLeft = Vector3Int.zero;
    private Vector3Int down = Vector3Int.zero;
    private Vector3Int downRight = Vector3Int.zero;

    private Vector2 dir = Vector2.zero; // 내가 tilePos를 기준으로 어느쪽에 있는가.
    private Camera mainCam = null;

    public Map map;
    [SerializeField] private Tower towerBase;

    private Dictionary<CoreType, CoreBase> coreDic = new Dictionary<CoreType, CoreBase>();
    public List<CoreBase> coreList = new List<CoreBase>();

    public UI_GuideText guideText;

    private void Start()
    {
        mainCam = Camera.main;

        foreach(var item in coreList)
        {
            coreDic.Add(item.coreType, item);
        }
    }

    private void Update()
    {
        SetCurTilePos();
        SetDir();
        SetAroundTiles();
    }


    public void SetDir()
    {
        Vector2 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        dir = map.tilemap.CellToWorld(new Vector3Int((int)pos.x, (int)pos.y, 0)) - map.tilemap.CellToWorld(tilePos);
    }

    public void SetAroundTiles()
    {
        upLeft = new Vector3Int(tilePos.x - 1, tilePos.y + 1, tilePos.z);
        up = new Vector3Int(tilePos.x, tilePos.y + 1, tilePos.z);
        upRight = new Vector3Int(tilePos.x + 1, tilePos.y + 1, tilePos.z);
        left = new Vector3Int(tilePos.x - 1, tilePos.y, tilePos.z);
        curPos = new Vector3Int(tilePos.x, tilePos.y, tilePos.z);
        right = new Vector3Int(tilePos.x + 1, tilePos.y, tilePos.z);
        downLeft = new Vector3Int(tilePos.x - 1, tilePos.y - 1, tilePos.z);
        down = new Vector3Int(tilePos.x, tilePos.y - 1, tilePos.z);
        downRight = new Vector3Int(tilePos.x + 1, tilePos.y - 1, tilePos.z);
    }

    // 마우스 위치에 있는 타일
    public void SetCurTilePos()
    {
        Vector2 pos = mainCam.ScreenToWorldPoint(Input.mousePosition);
        tilePos = map.tilemap.WorldToCell(pos);    // 마우스 위치에 위치한 타일 받아오기
    }

    public void ResetCheckedTiles() // 전에 색을 바꿔주었던 친구들은 다시 리셋
    {
        if (checkedPos == null) return; // 처음에 널이라 오류
        foreach (var pos in checkedPos) map.tilemap.SetColor(pos, Color.white);
    }

    public void SetTilesColor(PlaceTileType placeTileType)
    {
        ResetCheckedTiles();

        Vector3Int[] checkPos = Get2By2Tiles();

        foreach (var pos in checkPos)
        {
            if (IsPlaceableTile(pos, placeTileType))
            {
                map.tilemap.SetColor(pos, new Color(0, 1, 0, 0.5f)); // 아마 블루
            }
            else
            {
                map.tilemap.SetColor(pos, new Color(1, 0, 0, 0.5f)); // 아마 레드
            }
        }

        checkedPos = checkPos; // 내가 체크할 포지션들을 나중에 지워주야
    }

    public bool CanPlace(PlaceTileType placeTileType) // 2x2 타일 검사
    {
        bool canPlace = true;

        foreach (var pos in Get2By2Tiles())
        {
            if (IsPlaceableTile(pos, placeTileType) == false)
            {
                canPlace = false;
                break;
            }
        }
        return canPlace;
    }

    public Vector3Int[] Get2By2Tiles()
    {
        if (dir.x > 0 && dir.y > 0) // 1사분면
            return new Vector3Int[4] { curPos, right, upRight, up };

        else if (dir.x < 0 && dir.y > 0)// 2사분면
            return new Vector3Int[4] { curPos, left, upLeft, up };

        else if (dir.x < 0 && dir.y < 0)// 3사분면
            return new Vector3Int[4] { curPos, left, downLeft, down  };

        else  // 4사분면
            return new Vector3Int[4] { curPos, right, downRight, down };

    }

    public bool IsPlaceableTile(Vector3Int pos, PlaceTileType placeTileType)
    {
        if (pos.x < 0 || pos.y < 0) return false;
        if (pos.x >= map.width || pos.y >= map.height) return false;

        switch(placeTileType)
        {
            case PlaceTileType.Place:
                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Place) return false;
                break;
            case PlaceTileType.Road:
                if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Road) return false;
                break;
        }

        //여기까지 오면 OK! 타워가 깔릴 수 있는 타입과, 내가 깔려고 하는 타일의 타일타입이 같은거니까 설치 가능.
        return true;
    }   
    

    //준서놈
    public Vector3 Get2By2TilesCenter(Vector3Int[] targetTiles)
    {
        float x, y;

        x = (float)(targetTiles[0].x + targetTiles[1].x) / 2;
        y = (float)(targetTiles[2].y + targetTiles[3].y) / 2;

        return new Vector3(x - (float)(map.width-1) / 2, y - (float)(map.height-1) / 2, targetTiles[0].z); // 그리드를 이동시켰기 때문에, 이제 이동시킨 값만큼 보내줘야 해
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(TowerSO towerSO)
    {
        Tower newTower = Instantiate(towerBase, Get2By2TilesCenter(Get2By2Tiles()), Quaternion.identity);
        newTower.InitTowerData(towerSO);

        CoreBase newCore = Instantiate(coreDic[towerSO.coreType]);
        newCore.transform.SetParent(newTower.transform);
        newCore.transform.position = newTower.coreTrm.position;
        newCore.towerData = newTower.TowerData;

        //원래는 타일 값에 따라 받아와야 하지만, 어차피 타일 값 == 포지션 값이니까..

        foreach(var item in newTower.GetComponentsInChildren<SpriteRenderer>())
        {
            item.sortingOrder = map.height - (int)newTower.transform.position.y;
        }

        foreach (var pos in checkedPos) // 2x2타일은 타워 설치한 칸으로 설정해주고.
        {
            map.mapTileTypeArray[pos.x, pos.y] = TileType.Tower;
        }
    }
}
