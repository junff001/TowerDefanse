using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class BuildManager : Singleton<BuildManager>
{
    [SerializeField] private Tilemap tilemap = null;               // 타일맵
    [SerializeField] private Tilemap towerTileamp = null;          // 타워 타일맵
    [SerializeField] private Sprite roadSpite = null;              // 길 스프라이트
    [SerializeField] private Sprite hillSprite = null;             // 언덕 스프라이트
    [SerializeField] private Sprite towerSprite = null;            // 타워 스프라이트
    [SerializeField] private LayerMask towerMask = default;

    public GameObject towerPrefab = null;                          // 타워 프리팹

    private Tile currentTile = null;                               // 마우스에 위치한 현재 타일
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    private Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치
    private List<GameObject> towerList = new List<GameObject>();

    void Update()
    {
        TileInMousePosition();
        TileChecking();
    }

    // 타워를 스폰하는 함수
    void SpawnTower(Vector3Int tilePos)
    {
        // Tower 가 SpawnTile 자식으로 들어감
        // 중복 생성 방지
        if (GoldManager.Instance.GoldMinus(100))
        {
            if (towerList.Count == 0)
            {
                towerList.Add(Instantiate(towerPrefab, tilemap.GetCellCenterWorld(tilePos), Quaternion.identity));
            }
            else
            {
                for (int i = 0; i < towerList.Count; i++)
                {
                    if (towerList[i].transform.position == tilemap.GetCellCenterWorld(tilePos))
                    {
                        return;
                    }
                }

                towerList.Add(Instantiate(towerPrefab, tilemap.GetCellCenterWorld(tilePos), Quaternion.identity));
            }
        }
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
                    for (int i = 0; i < towerList.Count; i++)
                    {
                        if (towerList[i].transform.position == tilemap.GetCellCenterWorld(tilePos))
                        {
                            towerList[i].GetComponent<Tower>().TowerUpgrade();
                        }
                    }

                    SpawnTower(tilePos);
                }
            }
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
}
