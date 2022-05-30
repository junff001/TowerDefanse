using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class Map : MonoBehaviour
{
    public Tilemap tilemap = null;        // 타일맵

    public TileType[,] mapTileTypeArray = null;
    public Tile[,] mapTileArray = null;

    public int width;
    public int height;

    private void Start()
    {
        width = tilemap.size.x;
        height = tilemap.size.y;

        InitMap(width, height);
    }

    public void ResetMapData() // 그 전환 시 배열 데이타 다시 사용하기 위해서 초기화
    { 
        for(int x =0; x< width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if(mapTileTypeArray[x,y] == TileType.Road_Tower)
                {
                    mapTileTypeArray[x, y] = TileType.Place;
                }
                if (mapTileTypeArray[x, y] == TileType.Place_Tower)
                {
                    mapTileTypeArray[x, y] = TileType.Road;
                }
            }
        }
    }

    void InitMap(int width, int height)
    {
        mapTileTypeArray = new TileType[width, height];                 // 맵 사이즈 설정
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

            tilemap.SetTileFlags(position, TileFlags.None);

            x++;
        }
    }

    TileType GetTileType(Tile tile)
    {
        if (tile.sprite.name.Contains("Road")) // 나중에 읽기 편하라고 
            return TileType.Road;
        else if(tile.sprite.name.Contains("Place")) 
            return TileType.Place;
        else
            return TileType.Obstacle;
    }
}
