using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class ActData
{
    public Define.ActType actType = Define.ActType.Enemy;
    public Define.MonsterType monsterType  = Define.MonsterType.None;
    public Define.SpeciesType speciesType  = Define.SpeciesType.None;

    public ActData(Define.ActType actType, Define.MonsterType monsterType, Define.SpeciesType speciesType)
    {
        this.actType = actType;
        this.monsterType = monsterType;
        this.speciesType = speciesType;
    }
}
