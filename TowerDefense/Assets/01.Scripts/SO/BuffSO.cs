using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "ScriptableObjects/BuffData")]
public class BuffSO : ScriptableObject
{
    public float Damage { get; set; }
    public float Speed { get; set; }
    public float Duration { get; set; }
    public float Radius { get; set; }

    public ParticleSystem AuraEffect { get; set; }
    public ParticleSystem BuffEffect { get; set; }
}

public struct BuffData
{
    public float Damage;
    public float Speed;
    public float Duration;
    public float Radius;

    public ParticleSystem AuraEffect;
    public ParticleSystem BuffEffect;
}
