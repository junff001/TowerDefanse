using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnDamaged;
    public Action OnDied;

    private EnemyBase enemyBase;
    [SerializeField]
    private float healthAmountMax = 100;
    private float curHealthAmount
    {
        get
        {
            return enemyBase.myStat.HP;
        }

        set
        {
            enemyBase.myStat.HP = value;
        }
    }

    private float shieldhAmountMax = 100;
    private float curShieldAmount
    {
        get
        {
            return enemyBase.myStat.Shield;
        }

        set
        {
            enemyBase.myStat.Shield = value;
        }
    }

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        curHealthAmount = healthAmountMax;
    }

    public void Damage(float damageAmount)
    {
        curHealthAmount -= damageAmount;
        curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);
    }

    public bool IsDead()
    {
        return curHealthAmount == 0;
    }

    public bool IsFullHealth()
    {
        return curHealthAmount == healthAmountMax;
    }

    public float GetHealthAmount()
    {
        return curHealthAmount;
    }

    public float GetHealthAmountNormalized()
    {
        return (float)curHealthAmount / healthAmountMax;
    }

    public void SetHealthAmountMax(int hpAmountMax, bool updateHpAmount)
    {
        healthAmountMax = hpAmountMax;
        if (updateHpAmount)
        {
            curHealthAmount = hpAmountMax;
        }
    }

    public void TakeDamage(float damageAmount)
    {
        //transform.GetComponent<EnemyBase>().EnemyFlashStart();

        Damage(damageAmount);
        OnDamaged?.Invoke();

        if (IsDead())
        {
            OnDied?.Invoke();
        }
    }

}