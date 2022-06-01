using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActData
{
    public Define.ActType actType = Define.ActType.Enemy;
    public Define.MonsterType monsterType  = Define.MonsterType.None;
    public int spawnCost = 1;

    public ActData(Define.ActType actType, Define.MonsterType monsterType = Define.MonsterType.None, int spawnCost = 1)
    {
        this.actType = actType;
        this.monsterType = monsterType;
        this.spawnCost = spawnCost;
    }
}
