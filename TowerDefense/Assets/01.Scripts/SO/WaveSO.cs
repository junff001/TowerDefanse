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
    public string infoName; // 가독성 높이려고...
    public SpawnerMonsterCount[] monsterBox;
    [Header("보스")]
    public EnemyBase boss; // 보스 스크립트로? 확정 아직 아님, 없다면 null로
}

[System.Serializable]
public class SpawnerMonsterCount
{
    [Header("적 종류")]
    public EnemyBase enemy; // HealthSystem? 적 스크립트? 확정 아직 아님
    [Header("적 카운트")]
    public int enemyCount;
}
