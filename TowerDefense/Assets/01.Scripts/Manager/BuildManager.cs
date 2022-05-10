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

    public GameObject towerPrefab = null;                          // 타워 프리팹
    public bool isTowerSetting { get; set; } = false;              // 타워 정보 설정 할 것인가
    public Color towerColor { get; set; } = default;               // 타워 색깔

    private Tile currentTile = null;                               // 마우스에 위치한 현재 타일
    private Vector3Int tilePos = Vector3Int.zero;                  // 타일 위치
    private Vector3Int beforeTilePos = Vector3Int.zero;            // 이전 타일 위치
    private List<GameObject> towerList = new List<GameObject>();   // 타워 위치를 관리할 리스트
    private GameObject currentTower = null;

    public Transform grid;

    void Update()
    {
        TileInMousePosition();

        if (isTowerSetting)
        {
            TowerSetting();
            isTowerSetting = false;
        }

        TileChecking();
    }

    // 타워를 스폰하는 함수
    public void SpawnTower(Vector3Int tile)
    {
        // 중복 생성 방지
        for (int i = 0; i < towerList.Count; i++)  // 현 타일 위치에 타워 존재여부 확인
        {
            if (towerList[i].transform.position == tilemap.GetCellCenterWorld(tilePos))
            {
                return;
            }
        }

        towerList.Add(currentTower);

        if (towerList[towerList.Count - 1].transform.position == waitTrm.position)
        {
            if (GoldManager.Instance.GoldMinus(100))
            {
                towerList[towerList.Count - 1].transform.position = tilemap.GetCellCenterWorld(tilePos);
            }
        }
    }

    // 타워 세팅하는 함수
    void TowerSetting()
    {
        currentTower = Instantiate(towerPrefab);
        currentTower.transform.position = waitTrm.position;
        currentTower.GetComponent<Tower>().spriteRenderer.color = towerColor;
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
                    for (int i = 0; i < towerList.Count; i++) // 똑같은 타워를 눌렀는가
                    {
                        if (towerList[i].transform.position == tilemap.GetCellCenterWorld(tilePos))
                        {
                            //towerList[i].GetComponent<Tower>().TowerUpgrade();
                        }
                    }

                    if (currentTower != null)
                    {
                        SpawnTower(tilePos);
                    }
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

    public void SetTilemap(GameObject curStageTilemap)
    {
        Instantiate(curStageTilemap, Vector3.zero, Quaternion.identity, grid);
    }
}
