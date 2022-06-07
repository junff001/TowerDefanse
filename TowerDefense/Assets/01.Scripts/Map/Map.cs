using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{
    [HideInInspector] public Tilemap tilemap = null;        // 타일맵
    public Tilemap tilemap_view = null;        // 타일맵

    public Define.TileType[,] mapTileTypeArray = null;
    public Tile[,] mapTileArray = null;

    public int width;
    public int height;

    private void Awake()
    {
        if(tilemap == null) tilemap = GetComponent<Tilemap>();
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
        if (tile.sprite.name.Contains("Road")) // 나중에 읽기 편하라고 
            return Define.TileType.Road;
        else if(tile.sprite.name.Contains("Place")) 
            return Define.TileType.Place;
        else
            return Define.TileType.Obstacle;
    }
}
