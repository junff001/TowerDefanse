using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ActData
{
    public ActType actType { get; private set; } = ActType.None;
    public MonsterType monsterType { get; private set; } = MonsterType.None;

    public ActData(ActType actType, MonsterType monsterType)
    {
        this.actType = actType;
        this.monsterType = monsterType;
    }
}
