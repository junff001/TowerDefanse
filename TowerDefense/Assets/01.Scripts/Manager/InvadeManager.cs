using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvadeManager : Singleton<InvadeManager>
{
    public Queue<ActData> actQueue = new Queue<ActData>(); // 몹 편성 눌러서 여기에 추가.

    public void Spawn(MonsterType monsterType)
    {
        switch(monsterType)
        {
            case MonsterType.Goblin:
                Debug.Log("Goblin");
            break;
            case MonsterType.Ghost:
                Debug.Log("Ghost");
                break;
        }

        TryAct();
    }

    public void CheckActType(ActData actData)
    {
        switch(actData.actType)
        {
            case ActType.Wait:
                StartCoroutine(Wait());
                break;

            case ActType.Enemy:
                Spawn(actData.monsterType);
                break;
        }
    }

    public void WaveStart() //TryAct라는 말이 웨이브 시작할 때 실행할 함수명으로 적절치 않아서 그냥 WaveStart라고 따로 만들어뒀어여
    {
        TryAct(); 
    }

    IEnumerator Wait()
    {
        Debug.Log("1초 대기");
        yield return new WaitForSeconds(1f);

        TryAct();
    }

    void TryAct()
    {
        if (actQueue.Count > 0)
        {
            CheckActType(actQueue.Dequeue());
        }
    }
}
