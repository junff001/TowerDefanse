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
    public string infoName; // ������ ���̷���...
    public SpawnerMonsterCount[] monsterBox;
    [Header("����")]
    public EnemyBase boss; // ���� ��ũ��Ʈ��? Ȯ�� ���� �ƴ�, ���ٸ� null��
}

[System.Serializable]
public class SpawnerMonsterCount
{
    [Header("�� ����")]
    public EnemyBase enemy; // HealthSystem? �� ��ũ��Ʈ? Ȯ�� ���� �ƴ�
    [Header("�� ī��Ʈ")]
    public int enemyCount;
}
