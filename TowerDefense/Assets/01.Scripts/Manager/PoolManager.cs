using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PoolManager
{
    public Transform poolInitPos;

    private Dictionary<string, IPool> poolDic = new Dictionary<string, IPool>();

    public void CreatePool<T>(GameObject prefab, Transform parent, int count = 5) where T : MonoBehaviour
    {
        Type t = typeof(T);

        ObjectPooling<T> pool = new ObjectPooling<T>(prefab, parent, count);
        
        poolDic.Add(t.ToString(), pool);
    }

    public T GetItem<T>() where T : MonoBehaviour
    {
        Type t = typeof(T);
        ObjectPooling<T> pool = (ObjectPooling<T>)poolDic[t.ToString()];
        return pool.GetOrCreate();
    }

    public void Init(Transform initPos)
    {
        poolInitPos = initPos;

        GameObject arrow = Resources.Load<GameObject>("Bullet/Arrow");
        CreatePool<Arrow>(arrow, initPos, 50);

        GameObject bomb = Resources.Load<GameObject>("Bullet/Bomb");
        CreatePool<Bomb>(bomb, initPos, 50);

        GameObject canonBall = Resources.Load<GameObject>("Bullet/Canon Ball");
        CreatePool<CanonBall>(canonBall, initPos, 50);

        // 이펙트
        GameObject effectStoneFragment = Resources.Load<GameObject>("Effects/Effect_StoneFragment");
        CreatePool<Effect_StoneFrag>(effectStoneFragment, initPos, 5);
    }

    public void Clear()
    {
        poolDic.Clear();
    }
}
