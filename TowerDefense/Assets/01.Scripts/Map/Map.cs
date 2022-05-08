using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Map : MonoBehaviour
{
    
    [SerializeField] private Tilemap tilemap = null;        // 타일맵

    private int width = 0;                                  // 맵 가로 크기
    private int height = 0;                                 // 맵 세로 크기
    private TileBase[,] map = null;                         // 맵 2차원 배열
    private TileBase[] tiles = null;                        // 맵에서 뽑아낸 타일들

   

    //void InitMap()
    //{
    //    map = new TileBase[width, height];                 // 맵 사이즈 설정
    //    int tileCount = width * height;

    //    for (int i = 0; i < tileCount; i++)
    //    {
    //        for (int x = 0; x < width; x++)
    //        {
    //            for (int y = 0; y < height; y++)s
    //            {
    //                map[x, y] = tiles[i].
    //            }
    //        }
    //    }
    //}
}
