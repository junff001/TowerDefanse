using System.Collections.Generic;
using UnityEngine;

public class StageData : MonoBehaviour
{
    public Transform waypointsParent;
    public Map map;
    public WaveSO waveSO;

    public List<IndexWayPointList> pointLists = new List<IndexWayPointList>();
}

[System.Serializable]
public class IndexWayPointList
{
    public List<int> indexWayPoints = new List<int>();
}