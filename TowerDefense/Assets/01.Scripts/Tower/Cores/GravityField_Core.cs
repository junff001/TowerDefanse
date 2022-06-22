using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityField_Core : CoreBase
{
    private List<GameObject> flyingEnemies = new List<GameObject>();

    public override IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies.Count > 0);
            Attack(0, null);
            yield return new WaitForSeconds(1f / TowerData.AttackSpeed);
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        for (int i = 0; i < enemies.Count; ++i)
        {
            if (enemies[i] != null)
            {
                enemies[i].AddBuff(new Slow(enemies[i].gameObject, 1 / TowerData.AttackSpeed, enemies[i].enemyData.MoveSpeed * 0.3f));


                //비행몹인지 체크할 필요 있음.

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
                    enemies[i].GetComponent<SpineController>().FallDown();
                }
            }
        }
    }

}
