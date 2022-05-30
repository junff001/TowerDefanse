using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActData
{
    public ActType actType = ActType.Enemy;
    public MonsterType monsterType  = MonsterType.None;
    public int spawnCost = 1;

    public ActData(ActType actType, MonsterType monsterType = MonsterType.None, int spawnCost = 1)
    {
        this.actType = actType;
        this.monsterType = monsterType;
        this.spawnCost = spawnCost;
    }
}
