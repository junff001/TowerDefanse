using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveData")]
public class WaveSO : ScriptableObject
{
    public SpawnerInfo[] waveEnemyInfos;
}

[System.Serializable]
public class SpawnerInfo
{
    public string infoName; // 가독성 떨어질까봐...
    public SpawnerMonsterCount[] monsterBox;
}

[System.Serializable]
public class SpawnerMonsterCount
{
    [Header("적들")]
    public EnemyBase enemy;
    [Header("적 수")]
    public int enemyCount;
    [Header("웨이 포인트 인덱스")]
    public int wayPointListIndex;
}
