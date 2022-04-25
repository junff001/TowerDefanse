using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    [SerializeField] private GameObject wayPointPrefab = null;              // Prefab
    [SerializeField] private Tilemap roadTilemap = null;                    // 로드 타일맵

    public List<Tile> cornerTiles = null;                                   // 코너 타일
    public List<Tile> mapCornerTiles = new List<Tile>();                    // 맵에 존재하는 코너 타일
    [HideInInspector] 
    public List<GameObject> wayPoints = new List<GameObject>();             // 생성된 wayPoint

    void Awake()
    {
        Instance = this;
        WayPointSetting();
    }

    // WayPoint 를 코너 타일에 배치 하는 함수
    void WayPointSetting()
    {
        FindCornerTileAtMap();

        for (int i = 0; i < mapCornerTiles.Count; i++)
        {
            wayPoints.Add(Instantiate(wayPointPrefab));
            wayPoints[i].transform.position = mapCornerTiles[i].gameObject.transform.position;
        }
    }

    // 로드 타일맵에서 코너 타일 찾는 함수
    void FindCornerTileAtMap()
    {
        TileBase[] TileMap = roadTilemap.GetTilesBlock(roadTilemap.cellBounds);

        for (int i = 0; i < TileMap.Length; i++)
        {
            if (TileMap[i].Equals(cornerTiles[i]))
            {
                mapCornerTiles.Add((Tile)TileMap[i]);
            }
        }
    }
}
