using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject bulletPrefab;

    private void Awake()
    {
        PoolManager.CreatePool<Bullet>(bulletPrefab, transform, 50);
    }

    private void OnDestroy()
    {
        PoolManager.ResetPool();
    }
}
