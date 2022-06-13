using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Bullet
{
    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        base.Init(towerData,enemyTrm);
        gameObject.SetActive(true);
    }

    public override void Update()
    {
        base.Update();
    }
}
