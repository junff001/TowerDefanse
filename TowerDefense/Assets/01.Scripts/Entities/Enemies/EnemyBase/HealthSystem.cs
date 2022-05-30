using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum eHealthType
{
    HEALTH,
    SHIELD
}

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
            return enemyBase.enemyData.HP;
        }

        set
        {
            enemyBase.enemyData.HP = value;
        }
    }

    private float shieldAmountMax = 100;
    private float curShieldAmount
    {
        get
        {
            return enemyBase.enemyData.Shield;
        }

        set
        {
            enemyBase.enemyData.Shield = value;
        }
    }

    private void Awake()
    {
        enemyBase = GetComponent<EnemyBase>();
        curHealthAmount = healthAmountMax;
    }

    private void Damage(float damageAmount, bool penetration)
    {
        float tempAmount = curShieldAmount;

        curShieldAmount -= damageAmount;
        curShieldAmount = Mathf.Clamp(curShieldAmount, 0, shieldAmountMax);

        if(curShieldAmount <= 0)
        {
            float realDamage = damageAmount - tempAmount; // 데미지 초과 분량
            curHealthAmount -= realDamage;
            curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);
        }

        if (penetration)
        {
            curHealthAmount -= damageAmount;
            curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);
        }
    }

    public bool IsDead()
    {
        return curHealthAmount == 0;
    }

    public bool IsFullValue(eHealthType type)
    {
        return (type == eHealthType.HEALTH) ? curHealthAmount == healthAmountMax : curShieldAmount == shieldAmountMax;
    }

    public float GetAmount(eHealthType type)
    {
        return (type == eHealthType.HEALTH) ? curHealthAmount : curShieldAmount;
    }

    public float GetAmountNormalized(eHealthType type)
    {
        return (type == eHealthType.HEALTH) ? (float)curHealthAmount / healthAmountMax : (float)curShieldAmount / shieldAmountMax;
    }

    public void SetAmountMax(eHealthType type, int amountMax, bool updateAmount)
    {
        if (type == eHealthType.HEALTH)
        {
            healthAmountMax = amountMax;
            if (updateAmount)
            {
                curHealthAmount = amountMax;
            }
        }
        else
        {
            shieldAmountMax = amountMax;
            if (updateAmount)
            {
                curShieldAmount = amountMax;
            }
        }
    }

    public void TakeDamage(float damageAmount, bool penetration = false)
    {
        Damage(damageAmount, penetration);
        OnDamaged?.Invoke();

        if (IsDead())
        {
            OnDied?.Invoke();
        }
    }

}