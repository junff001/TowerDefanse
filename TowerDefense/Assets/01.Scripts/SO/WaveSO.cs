using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveData")]
public class WaveSO : ScriptableObject
{
    public SpawnerInfo[] waveEnemyInfos;

    [Header("오펜스 때 끊어나올 수")]
    public int offenseHeadCount = 5;
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
    [Header("적 특성")]
    public Define.MonsterType monsterType;
    [Header("적 종족")]
    public Define.SpeciesType speciesType;
    [Header("웨이 포인트 인덱스")]
    public int wayPointListIndex;
}
