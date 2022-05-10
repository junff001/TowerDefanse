using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : EnemyBase
{
    public override void WaveStatControl(int wave)
    {
        float value_f = (wave * Mathf.Pow(1.5f, 0)) * 100 * 1.5f;
        int value = (int)value_f;

        healthSystem.SetHealthAmountMax(value, true); // 체력 조절
    }
}
