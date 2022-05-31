using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectPoolManager : MonoBehaviour
{
    public GameObject arrowPrefab;
    public GameObject cannonBallPrefab;
    public GameObject bombPrefab;

    [Header("Effects")]
    public GameObject effectStoneFragment;

    private void Awake()
    {
        PoolManager.CreatePool<Arrow>(arrowPrefab, transform, 50);
        PoolManager.CreatePool<CannonBall>(cannonBallPrefab, transform, 50);
        PoolManager.CreatePool<Bomb>(bombPrefab, transform, 50);
        PoolManager.CreatePool<Effect_StoneFrag>(effectStoneFragment, transform, 5);
    }

    private void OnDestroy()
    {
        PoolManager.ResetPool();
    }
}
