using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class SpawnOrWait
{
    ActType actType = ActType.None;
    MonsterType monsterType = MonsterType.None;

    public SpawnOrWait(ActType actType, MonsterType monsterType)
    {
        this.actType = actType;
        this.monsterType = monsterType;
    }
}
