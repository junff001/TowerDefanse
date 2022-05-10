using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private HealthSystem healthSystem;

    private Transform barTrm;

    private void Awake()
    {
        barTrm = transform.Find("bar");
    }

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

    private void CallHealthSystemOnDamaged()
    {
        UpdateBar();
        UpdateHealthBarVisible();
    }

    private void UpdateBar()
    {
        Debug.Log("바 추가됨");
        barTrm.localScale = new Vector3(healthSystem.GetHealthAmountNormalized(), 1, 1);
    }

    private void UpdateHealthBarVisible()
    {
        if (healthSystem.IsFullHealth())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
