using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    [HideInInspector] public Tilemap tilemap = null;        // 타일맵
    public Tilemap tilemap_view = null;        // 타일맵

    public Define.TileType[,] mapTileTypeArray = null;
    public Tile[,] mapTileArray = null;

    public int width;
    public int height;

    public Tile placeTile;
    public Tile roadTile;
    public Tile waterTile;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        if(tilemap_view == null)
            tilemap_view = tilemap;

        InitMap();
    }

    public void ResetMapData() // 그 전환 시 배열 데이타 다시 사용하기 위해서 초기화
    { 
        for(int x =0; x< width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(mapTileTypeArray[x,y] == Define.TileType.Road_Tower)
                {
                    mapTileTypeArray[x, y] = Define.TileType.Place;
                }
                if (mapTileTypeArray[x, y] == Define.TileType.Place_Tower)
                {
                    mapTileTypeArray[x, y] = Define.TileType.Road;
                }
            }
        }
    }

    public void InitMap()
    {
        placeTile = Managers.Build.placeTile;
        roadTile = Managers.Build.roadTile;
        waterTile = Managers.Build.waterTile;

        width = tilemap.size.x;
        height = tilemap.size.y;

        mapTileTypeArray = new Define.TileType[width, height];                 // 맵 사이즈 설정
        mapTileArray = new Tile[width, height];                 // 맵 사이즈 설정

        int x = 0, y = 0;
        foreach (var position in tilemap.cellBounds.allPositionsWithin)
        {
            if(x >= width)
            {
                y++;
                x = 0;
            }
            if (!tilemap.HasTile(position))
            {
                continue;
            }

            Tile tile = tilemap.GetTile<Tile>(position);
            mapTileArray[x, y] = tile;
            mapTileTypeArray[x, y] = GetTileType(tile);
            x++;
        }

        foreach (var position in tilemap_view.cellBounds.allPositionsWithin)
        {
            tilemap_view.SetTileFlags(position, TileFlags.None); // 변하게 하는건 보여지는 Tilmeap_View니까
        }
    }

    Define.TileType GetTileType(Tile tile)
    {
        if (tile == roadTile) // 나중에 읽기 편하라고 
            return Define.TileType.Road;

        else if (tile == placeTile)
            return Define.TileType.Place;

        else if (tile == waterTile)
            return Define.TileType.Water;

        else
            return Define.TileType.Obstacle;
    }
}
