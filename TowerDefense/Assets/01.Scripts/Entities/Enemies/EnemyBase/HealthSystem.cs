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
    public Action<float> OnMaxHealed;
    public Action OnDamaged;
    public Action OnDied;
    public bool canDamaged { get; set; } = true;

    public float damagedDelay { get; set; } = 0f;

    private EnemyBase enemyBase;
    private float healthAmountMax;
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

    private float shieldAmountMax;
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
    }

    void Start()
    {
        StartCoroutine(DotDamage());
    }

    IEnumerator DotDamage()
    {
        while (true)
        {
            if (!canDamaged)
            {
                yield return new WaitForSeconds(damagedDelay);
                canDamaged = true;
            }
            else
            {
                yield return null;
            }  
        }
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
        return curHealthAmount <= 0;
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
        if (type == eHealthType.HEALTH)
        {
            return (float)curHealthAmount / healthAmountMax;
        }
        else
        {
            if (shieldAmountMax != 0)
            {
                return (float)curShieldAmount / shieldAmountMax;
            }
            else
            {
                return 0;
            }
        }
    }

    public void SetAmountMax(eHealthType type, int amountMax, bool updateAmount)
    {
        if (type == eHealthType.HEALTH)
        {
            healthAmountMax = amountMax;
            OnMaxHealed(healthAmountMax);
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

    public void TakeDamage(float damageAmount, Define.PropertyType property, bool penetration = false)
    {
        Damage(damageAmount, penetration);
        OnDamaged?.Invoke();

        if (IsDead())
        {
            OnDied?.Invoke();
        }
    }
}