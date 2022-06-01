using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    private HealthSystem healthSystem;

    [SerializeField] private Transform healthBarTrm;
    [SerializeField] private Transform shieldBarTrm;

    private void Start()
    {
        if(!healthSystem)
        {
            healthSystem = transform.parent.GetComponent<HealthSystem>();
        }

        healthSystem.OnDamaged += CallHealthSystemOnDamaged;

        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void Update()
    {
        float xScale = healthSystem.transform.localScale.x;

        if (xScale > 0)
        {
            xScale /= xScale;
            xScale *= -1;
        }
        else
        {
            xScale /= xScale;
        }

        transform.localScale = new Vector3(-xScale, 1, 1);
    }

    private void CallHealthSystemOnDamaged()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        healthBarTrm.localScale = new Vector3(healthSystem.GetAmountNormalized(eHealthType.HEALTH), 1, 1);
        shieldBarTrm.localScale = new Vector3(healthSystem.GetAmountNormalized(eHealthType.SHIELD), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullValue(eHealthType.SHIELD) && healthSystem.IsFullValue(eHealthType.HEALTH))
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
