using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] private GameObject prefab = null;

    private Queue<Bullet> poolQueue = new Queue<Bullet>();

    void Awake()
    {
        Initialize(10);
    }

    private Bullet CreateNewObject()
    {
        var newObj = Instantiate(prefab, transform).GetComponent<Bullet>();
        newObj.gameObject.SetActive(false);
        return newObj;
    }

    private void Initialize(int count)
    {
        for (int i = 0; i < count; i++)
        {
            poolQueue.Enqueue(CreateNewObject());
        }
    }

    public static Bullet GetObject()
    {
        if (Instance.poolQueue.Count > 0)
        {
            var obj = Instance.poolQueue.Dequeue();
            obj.transform.SetParent(null);
            obj.gameObject.SetActive(true);
            return obj;
        }
        else
        {
            var newObj = Instance.CreateNewObject();
            newObj.transform.SetParent(null);
            newObj.gameObject.SetActive(true);
            return newObj;
        }
    }

    public static void ReturnObject(Bullet bullet)
    {
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(Instance.transform);
        Instance.poolQueue.Enqueue(bullet);
    }
}
