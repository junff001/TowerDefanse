using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static Define;

public class Map : MonoBehaviour
{
    [HideInInspector] public Tilemap tilemap = null;        // 타일맵
    [HideInInspector] public Tilemap gridTilemap = null;        // 타일맵

    [HideInInspector] public TilemapRenderer gridTilemapRenderer = null;        // 타일맵
    private List<TilemapRenderer> tilemapRenderers = new List<TilemapRenderer>();

    public Define.TileType[,] mapTileTypeArray = null;
    public Tile[,] mapTileArray = null;



    public int width;
    public int height;

    [HideInInspector] public Tile placeTile;
    [HideInInspector] public Tile roadTile;
    [HideInInspector] public Tile waterTile;
    [HideInInspector] public Tile placeableTunnelTile;

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
        gridTilemap = transform.GetChild(0).GetComponent<Tilemap>();
        gridTilemapRenderer = gridTilemap.GetComponent<TilemapRenderer>();

        for(int i = 0; i < transform.childCount; i++)
        {
            TilemapRenderer tr = transform.GetChild(i).GetComponent<TilemapRenderer>();
            if(tr != null)
            {
                tilemapRenderers.Add(tr);
            }
        }

        InitMap();
    }

    public void SetTilemapsColor(bool on)
    {
        Debug.Log(tilemapRenderers.Count);

        for(int i = 0; i < tilemapRenderers.Count; i++)
        {
            Debug.Log(tilemapRenderers[i].name);
            Color color = tilemapRenderers[i].material.color;
            tilemapRenderers[i].material.color = on 
                ? new Color(color.r, color.g, color.b, 1) 
                : new Color(color.r, color.g, color.b, 0);
        }
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

    public void ShowPlaceableTiles(PlaceTileType placeTileType)
    {
        Managers.Build.placingTileType = placeTileType;

        foreach (var pos in tilemap.cellBounds.allPositionsWithin)
        {
            if(Managers.Build.IsPlaceableTile(pos, placeTileType))
            {
                gridTilemap.SetColor(pos, Color.white);
            }
        }
    }

    public void InitMap()
    {
        placeTile = Managers.Build.placeTile;
        roadTile = Managers.Build.roadTile;
        waterTile = Managers.Build.waterTile;
        placeableTunnelTile = Managers.Build.placeableTunnelTile;

        width = tilemap.size.x;
        height = tilemap.size.y;

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

            gridTilemap.SetTileFlags(position, TileFlags.None);
            gridTilemap.SetColor(position, new Color(1, 1, 1, 0f));
        }
    }

    TileType GetTileType(Tile tile)
    {
        if (tile == roadTile)
            return TileType.Road;

        else if (tile == placeTile)
            return TileType.Place;

        else if (tile == waterTile)
            return TileType.Water_UseAsTunnel;

        else if (tile == placeableTunnelTile)
            return TileType.PlaceableTunnel;

        return TileType.None;
    }
}
