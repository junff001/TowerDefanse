using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu( menuName = "ScriptableObjects/StageDataSO",order = 0)]
public class StageDataSO : ScriptableObject
{
    public Transform waypointsParent;
    public GameObject gridObj;
    public WaveSO waveSO;
}
