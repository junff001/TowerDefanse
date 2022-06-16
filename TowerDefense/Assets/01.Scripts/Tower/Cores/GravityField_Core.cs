using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField_Core : CoreBase
{
    private List<GameObject> flyingEnemies = new List<GameObject>();


    LayerMask flyingEnemyLayer = default;
    LayerMask enemyLayer = default;
    string walkAnimName = string.Empty;
    string flyAnimName = string.Empty;

    private void Awake()
    {
        flyingEnemyLayer = LayerMask.NameToLayer("FlyingEnemy");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }
    public override void Attack(int power, HealthSystem enemy)
    {
        if(enemy.gameObject.layer == flyingEnemyLayer)
        {
            flyingEnemies.Add(enemy.gameObject);
            enemy.gameObject.layer = enemyLayer;
            enemy.GetComponent<SpineController>().sa.AnimationName = walkAnimName;
        }


        CheckDistWithEnemies();
    }

    private void CheckDistWithEnemies()
    {
        for(int i = 0; i< flyingEnemies.Count; i++)
        {
            if(flyingEnemies[i] == null)
            {
                flyingEnemies.Remove(flyingEnemies[i]);
                continue;
            }

            if (Vector2.Distance(flyingEnemies[i].transform.position, transform.position) > towerData.AttackRange)
            {
                flyingEnemies.Remove(flyingEnemies[i]);
                flyingEnemies[i].gameObject.layer = flyingEnemyLayer;
                flyingEnemies[i].GetComponent<SpineController>().sa.AnimationName = flyAnimName;
                --i; // 하나 지울거니까 하나 빼주장
            }
        }
    }
}
