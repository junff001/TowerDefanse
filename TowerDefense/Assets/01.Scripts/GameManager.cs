using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public Tilemap tilemap = null;

    void Awake()
    {
        Instance = this;
    }
}
