using UnityEngine;

public class SoSetter : MonoBehaviour
{
    [SerializeField] private EnemySO[] enemySOs;

    public void SetEnemySOsSpawnCost()
    {
        for(int i = 0; i < enemySOs.Length; i++)
        {
            enemySOs[i].spawnCost = GetCost_SpeciesType(i);
            enemySOs[i].spawnCost += GetCost_MonsterType(i);
        }
    }

    public int GetCost_SpeciesType(int index)
    {
        int retValue = 0;
        switch(enemySOs[index].speciesType)
        {
            case Define.SpeciesType.Goblin:
                retValue = 50;
                break;
            case Define.SpeciesType.None:
                Debug.Log($"None이 뜨면 안됩니다! 문제 SO : {enemySOs[index].name}");
                break;
        }

        return retValue;
    }

    public int GetCost_MonsterType(int index)
    {
        int retValue = 0;
        if ((enemySOs[index].monsterType & Define.MonsterType.Normal) != 0) retValue += 50;
        if ((enemySOs[index].monsterType & Define.MonsterType.Shield) != 0) retValue += 100;
        if ((enemySOs[index].monsterType & Define.MonsterType.Armor)  != 0) retValue += 150;
        if ((enemySOs[index].monsterType & Define.MonsterType.Hide)   != 0) retValue += 200;
        if ((enemySOs[index].monsterType & Define.MonsterType.Fly)    != 0) retValue += 250;

        return retValue;
    }

}