using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/WaveData")]
public class MapInfoSO : ScriptableObject
{
    [Header("스테이지 이름")]
    public string stageName;

    [Header("스테이지 스프라이트")]
    public Sprite stageSprite;

    [Header("웨이브 정보")]
    public SpawnerInfo[] waveEnemyInfos;

    [Header("오펜스 때 끊어나올 수")]
    public int offenseHeadCount = 5;
}

[System.Serializable]
public class SpawnerInfo
{
    public SpawnerMonsterCount[] monsterBox;
}

[System.Serializable]
public class SpawnerMonsterCount
{
    [Header("적들")]
    public EnemySO so;
    [Header("적 수")]
    public int enemyCount;
    [Header("웨이 포인트 인덱스")]
    public int wayPointListIndex;
}
