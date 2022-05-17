using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    [SerializeField] private Tilemap tilemap = null;               // 타일맵
    [SerializeField] private Sprite roadSpite = null;              // 길 스프라이트
    [SerializeField] private Sprite hillSprite = null;             // 언덕 스프라이트
    [SerializeField] private LayerMask towerMask = default;
    [SerializeField] private Transform waitTrm = null;             // 대기 지점

    private Tile currentTile = null;                               // 마우스에 위치한 현재 타일
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    private Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치
    private GameObject currentTower = null;

    public Transform grid;

    void Update()
    {
        TileInMousePosition();

        TileChecking();
    }

    // 타워 세팅하는 함수
    public void TowerSetting(GameObject prefab)
    {
        currentTower = Instantiate(prefab);
        currentTower.transform.position = waitTrm.position;
    }

    // 마우스 위치에 있는 타일
    void TileInMousePosition()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);     // 마우스 월드 위치 
        tilePos = tilemap.WorldToCell(pos);                                    // 마우스 위치에 위치한 타일을 Vector3Int 로 변환
        currentTile = tilemap.GetTile<Tile>(tilePos);
    }

    // 현재 타일이 뭔지 확인하는 함수
    void TileChecking()
    {
        if (currentTile != null)
        {
            if (currentTile.sprite == roadSpite)
            {
                tilemap.SetColor(beforeTilePos, Color.white);
            }
            else if (currentTile.sprite == hillSprite)
            {
                SpawnTileColorActive(); 

                if (Input.GetMouseButtonDown(0))
                {
                    if (currentTower != null)
                    {
                        SpawnTower();
                    }
                }
            }
        } 
    }

    // 타워를 스폰하는 함수
    public void SpawnTower()
    {
        TowerData towerData = currentTower.GetComponent<Tower>().TowerData;

        if (GoldManager.Instance.GoldMinus(towerData.PlaceCost))
        {
            currentTower.transform.position = tilemap.GetCellCenterWorld(tilePos);
        }
    }

    // 스폰 타일 컬러 활성화하는 함수
    void SpawnTileColorActive()
    {
        if (beforeTilePos != tilePos)
        {
            tilemap.SetColor(beforeTilePos, Color.white);
        }

        tilemap.SetTileFlags(tilePos, TileFlags.None);   // 잠금해제
        tilemap.SetColor(tilePos, Color.red);

        beforeTilePos = tilePos;
    }

    public void SetTilemap(GameObject curStageTilemap)
    {
        Instantiate(curStageTilemap, Vector3.zero, Quaternion.identity, grid);
    }
}
