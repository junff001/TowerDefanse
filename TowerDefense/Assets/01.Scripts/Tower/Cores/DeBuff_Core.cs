using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct BuffData
{
    // 버프
    public int addOffense;
    public float addAttackSpeed;
    public float addRange;

    // 디버프
    public float slow_percentage;
    public float addDotDamage;

    // 지속 시간
    public float duration;
}

public class DeBuff_Core : CoreBase
{
    [SerializeField] private BuffSO buffSO = null;
    private BuffData buffData = new BuffData();
    private SpriteRenderer sr;
    public LayerMask towerMask = default;

    private Collider2D[] towers = null;

    private Action<GameObject> buffAction;

    void Init(BuffSO buffSO)
    {
        buffData.addOffense = buffSO.AddOffense;
        buffData.addAttackSpeed = buffSO.AddAttackSpeed;
        buffData.addRange = buffSO.AddRange;
        buffData.slow_percentage = buffSO.Slow_Percentage;
        buffData.addDotDamage = buffSO.AddDotDamage;
        buffData.duration = buffSO.Duraion;
    }

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public override void OnEnable()
    {
        base.OnEnable();

        Init(buffSO);
    }

    public override IEnumerator OnRader()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            enemies = Rader(enemyMask);
            towers = Rader(towerMask);
        }
    }

    IEnumerator OnBuffTower()
    {
        while (true)
        {
            yield return new WaitUntil(() => towers?.Length > 0);

            for (int i = 0; i < towers.Length; i++)
            {
                if (towers[i] != null)
                {
                    buffAction.Invoke(towers[i].gameObject);
                }
            }

            yield return null;
        }
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
                    Attack(towerData.OffensePower, enemies[i].GetComponent<HealthSystem>());
                    buffAction.Invoke(enemies[i].gameObject);
                }
            }

            yield return null;
        }
    }

    public override void Attack(int power, HealthSystem enemy)
    {
        enemy.TakeDamage(towerData.OffensePower, towerData.Property);
    }

    public override void PropertyCheck()
    {
        switch (towerData.Property)
        {
            case Define.PropertyType.WATER:
                buffAction += Slow;
                break;
            case Define.PropertyType.FIRE:
               
                break;
            case Define.PropertyType.LIGHTNING:
                
                break;
            case Define.PropertyType.LIGHT:
                
                break;
            case Define.PropertyType.DARKNESS:
                buffAction += Dot;
                break;
        }
    }

    void Slow(GameObject target)
    {
        BuffSlow slow = new BuffSlow(target, buffData.duration, buffData.slow_percentage);
        target.GetComponent<EnemyBase>().AddBuff(slow);
    }

    void Dot(GameObject target)
    {
        BuffDOT dot = new BuffDOT(target, buffData.duration, buffData.addDotDamage);
        target.GetComponent<EnemyBase>().AddBuff(dot);
    }
}
