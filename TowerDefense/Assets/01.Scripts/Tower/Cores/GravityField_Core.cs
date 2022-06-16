using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField_Core : CoreBase
{
    private List<GameObject> flyingEnemies = new List<GameObject>();

    LayerMask flyingEnemyLayer = default;
    LayerMask enemyLayer = default;

    private void Awake()
    {
        flyingEnemyLayer = LayerMask.NameToLayer("FlyEnemy");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public override IEnumerator OnRader()
    {
        while (true)
        {
            CheckDistWithEnemies();
            EnemyRader(enemyMask);
            yield return new WaitForSeconds(0.1f);
        }
    }

    public override IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies.Count > 0);
            Attack(0, null);
            yield return new WaitForSeconds(1f / towerData.AttackSpeed);
        }
    }


    public override void Attack(int power, HealthSystem enemy)
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
            {
                enemies[i].AddBuff(new Slow(enemies[i].gameObject, 1 / towerData.AttackSpeed, enemies[i].enemyData.MoveSpeed * 0.3f));

                if (enemies[i].gameObject.layer.Equals(flyingEnemyLayer))
                {
                    bool bAddEnemy = true;
                    for(int j = 0; j < flyingEnemies.Count; j++)
                    {
                        if(flyingEnemies[j] == enemies[i])
                        {
                            bAddEnemy = false;
                            break;
                        }
                    }

                    if (bAddEnemy)
                    {
                        flyingEnemies.Add(enemies[i].gameObject);
                        enemies[i].gameObject.layer = enemyLayer;
                        enemies[i].GetComponent<SpineController>().SetAnim(true);
                    }
                }
            }
        }
    }

    private void CheckDistWithEnemies()
    {
        for (int i = 0; i < flyingEnemies.Count; i++)
        {
            if (flyingEnemies[i] != null && Vector2.Distance(flyingEnemies[i].transform.position, transform.position) > towerData.AttackRange)
            {
                flyingEnemies[i].gameObject.layer = flyingEnemyLayer;
                flyingEnemies[i].GetComponent<SpineController>().SetAnim(false);
                flyingEnemies.Remove(flyingEnemies[i]);
                --i;
            }
        }
    }
}
