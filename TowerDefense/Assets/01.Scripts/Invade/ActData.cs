using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActData
{
    public Define.ActType actType = Define.ActType.Enemy;
    public Define.MonsterType monsterType  = Define.MonsterType.None;

    public ActData(Define.ActType actType, Define.MonsterType monsterType = Define.MonsterType.None)
    {
        this.actType = actType;
        this.monsterType = monsterType;
    }
}
