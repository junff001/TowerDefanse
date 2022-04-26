using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BuildManager : Singleton<BuildManager>
{
    public GameObject towerPrefab = null;               // 타워 프리팹

    // 타워를 스폰하는 함수
    public void SpawnTower(Transform tileTransform)
    {
        Instantiate(towerPrefab, tileTransform);
    }
}
