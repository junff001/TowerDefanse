using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    public Action OnDamaged;
    public Action OnDied;

    [SerializeField]
    private float healthAmountMax = 100f;
    private float curHealthAmount;

    private void Awake()
    {
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
            GoldManager.Instance.GoldPlus(50);
            OnDied?.Invoke();
        }
    }

}