using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    #region private

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
    #endregion

    public Map map;
    [SerializeField] private Tower towerBase;

    private Dictionary<CoreType, CoreBase> coreDic = new Dictionary<CoreType, CoreBase>();
    public List<CoreBase> coreList = new List<CoreBase>();


    private void Start()
    {
        mainCam = Camera.main;

        foreach(var item in coreList)
        {
            coreDic.Add(item.coreType, item);
        }

        Debug.Log(coreDic.Count);
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

    public void SetTilesColor()
    {
        ResetCheckedTiles();

        Vector3Int[] checkPos = Get2By2Tiles();

        foreach (var pos in checkPos)
        {
            if (IsPlaceableTile(pos))
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

    public bool CanPlace() // 2x2 타일 검사
    {
        bool canPlace = true;

        foreach (var pos in Get2By2Tiles())
        {
            if (IsPlaceableTile(pos) == false)
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
            return new Vector3Int[4] { curPos, up, upRight, right };

        else if (dir.x < 0 && dir.y > 0)// 2사분면
            return new Vector3Int[4] { curPos, up, upLeft, left };

        else if (dir.x < 0 && dir.y < 0)// 3사분면
            return new Vector3Int[4] { left, downLeft, down, curPos };

        else  // 4사분면
            return new Vector3Int[4] { right, downRight, down, curPos };

    }

    public bool IsPlaceableTile(Vector3Int pos)
    {
        if (pos.x < 0 || pos.y < 0) return false;
        if (pos.x >= map.width || pos.y >= map.height) return false;
        if (map.mapTileTypeArray[pos.x, pos.y] != TileType.Place) return false;

        return true;
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(TowerSO towerSO)
    {
        Tower newTower = Instantiate(towerBase, map.tilemap.CellToWorld(tilePos), Quaternion.identity);
        newTower.InitTowerData(towerSO);

        CoreBase newCore = Instantiate(coreDic[towerSO.coreType]);
        newCore.transform.SetParent(newTower.transform);
        newCore.transform.position = newTower.coreTrm.position;
        newCore.towerData = newTower.TowerData;

        foreach (var pos in checkedPos) // 2x2타일은 타워 설치한 칸으로 설정해주고.
        {
            map.mapTileTypeArray[pos.x, pos.y] = TileType.Tower;
        }
    }
}
