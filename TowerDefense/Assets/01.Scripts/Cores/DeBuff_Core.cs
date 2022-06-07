using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeBuff_Core : CoreBase
{
    [SerializeField] private Sprite fireJewel = null;
    [SerializeField] private Sprite waterJewel = null;
    [SerializeField] private Sprite lightJewel = null;
    [SerializeField] private Sprite lightingJewel = null;
    [SerializeField] private Sprite darkessJewel = null;

    private SpriteRenderer sprite = null;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public override IEnumerator OnAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies?.Length > 0);
            currentTarget = enemies[0];

            for (int i = 0; i < enemies.Length; i++)
            {
                if (i >= towerData.attackTargetCount)
                    break;

                if (enemies[i] != null)
                {
                    if (enemies[i].GetComponent<HealthSystem>().canDamaged)
                    {
                        enemies[i].GetComponent<HealthSystem>().damagedDelay = towerData.AttackSpeed;
                        Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                        
                        enemies[i].GetComponent<HealthSystem>().canDamaged = false;
                    } 
                }
            }

            yield return null;
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(power);
    }

    public override void PropertyCheck()
    {
        switch (towerData.property)
        {
            case Define.PropertyType.WATER:
                sprite.sprite = waterJewel;
                break;
            case Define.PropertyType.FIRE:
                sprite.sprite = fireJewel;
                break;
            case Define.PropertyType.LIGHTNING:
                sprite.sprite = lightingJewel;
                break;
            case Define.PropertyType.LIGHT:
                sprite.sprite = lightJewel;
                break;
            case Define.PropertyType.DARKNESS:
                sprite.sprite = darkessJewel;
                break;
        }
    }
}
