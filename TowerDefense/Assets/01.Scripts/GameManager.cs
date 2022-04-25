using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance = null;

    public List<GameObject> wayPoints = new List<GameObject>();

    void Awake()
    {
        Instance = this;
    }

    
}
