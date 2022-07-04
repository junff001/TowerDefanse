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

    private LivingEntity livingEntity;
    private float healthAmountMax;
    private float curHealthAmount
    {
        get
        {
            return livingEntity.livingEntityData.HP;
        }

        set
        {
            livingEntity.livingEntityData.HP = value;
        }
    }

    private float shieldAmountMax;
    private float curShieldAmount
    {
        get
        {
            return livingEntity.livingEntityData.ShieldAmount;
        }

        set
        {
            livingEntity.livingEntityData.ShieldAmount = value;
        }
    }

    void Awake()
    {
        livingEntity = GetComponent<LivingEntity>();
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

    private void Damage(float damageAmount, bool hasPenetration)
    {
        float tempAmount = curShieldAmount;

        curShieldAmount -= damageAmount;
        curShieldAmount = Mathf.Clamp(curShieldAmount, 0, shieldAmountMax);

        if (curShieldAmount <= 0)
        {
            float realDamage = damageAmount - tempAmount; // 데미지 초과 분량
            curHealthAmount -= realDamage;
            curHealthAmount = Mathf.Clamp(curHealthAmount, 0, healthAmountMax);
        }

        if (hasPenetration)
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
            Debug.Log((float)healthAmountMax);
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
            OnMaxHealed?.Invoke(healthAmountMax);
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

    public void TakeDamage(float damageAmount,  bool hasPenetration = false)
    {
        Damage(damageAmount, hasPenetration);
        OnDamaged?.Invoke();

        if (IsDead())
        {
            OnDied?.Invoke();
        }
    }
}