using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PropertyData", menuName = "ScriptableObjects/PropertyData")]
public class PropertySO : ScriptableObject
{
    public ParticleSystem AuraEffect {get; private set;}
    public ParticleSystem BuffEffect { get; private set; }
}
