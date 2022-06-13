using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : Bullet
{
    public override void Init(TowerData towerData, Transform enemyTrm)
    {
        target = enemyTrm;
        propertyType = towerData.Property;
        bulletDamage = towerData.OffensePower;
        gameObject.SetActive(true);
    }
}
