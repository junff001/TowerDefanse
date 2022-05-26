using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActData
{
    public ActType actType = ActType.Enemy;
    public MonsterType monsterType  = MonsterType.None;

    public ActData(ActType actType, MonsterType monsterType = MonsterType.None)
    {
        this.actType = actType;
        this.monsterType = monsterType;
    }
}
