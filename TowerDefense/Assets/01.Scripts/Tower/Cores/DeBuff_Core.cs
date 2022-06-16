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
    public BuffSO buffSO = null;
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
        buffData.duration = buffSO.Duration;
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

    public void TowerRader()
    {
        towers = Physics2D.OverlapCircleAll(transform.position + new Vector3(0, raderHeight, 0), towerData.AttackRange, towerMask);
    }

    public override IEnumerator OnRader()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            EnemyRader(enemyMask);
            TowerRader();
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

    public override IEnumerator CoAttack()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemies.Count > 0);
            target = enemies[0];

            for (int i = 0; i < enemies.Count; i++)
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
        Slow slow = new Slow(target, buffData.duration, buffData.slow_percentage);
        target.GetComponent<EnemyBase>().AddBuff(slow);
    }

    void Dot(GameObject target)
    {
        Dot dot = new Dot(target, buffData.duration, buffData.addDotDamage);
        target.GetComponent<EnemyBase>().AddBuff(dot);
    }
}
