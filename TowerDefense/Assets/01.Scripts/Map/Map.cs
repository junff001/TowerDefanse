using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public enum TileType
{
    Road,
    Placeable,
    Tower,
    Obstacle
}

public class Map : MonoBehaviour
{
    private Tilemap tilemap = null;        // 타일맵

    private int width = 18;                                  // 맵 가로 크기
    private int height = 10;                                 // 맵 세로 크기
    private TileType[,] mapTileTypeArray = null;
    private Tile[,] mapTileArray = null;

    private void Start()
    {
        tilemap = GetComponent<Tilemap>();
        Debug.Log(tilemap.cellBounds.size);
        InitMap();
        //GetCellCountX();
    }

    int GetCellCountX() // 이거 만들기
    {
        float width = tilemap.GetTile<Tile>(new Vector3Int(1, 1, 0)).sprite.bounds.size.x;
        Debug.Log(tilemap.cellBounds.size.x / width);

        return (int)(tilemap.cellBounds.size.x / width);

    }

    void InitMap()
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

            x++;
        }
    }

    TileType GetTileType(Tile tile)
    {
        if (tile.sprite.name.Contains("Road")) 
            return TileType.Road;

        else if(tile.sprite.name.Contains("Place")) 
            return TileType.Placeable;

        else
            return TileType.Obstacle;
    }
}
