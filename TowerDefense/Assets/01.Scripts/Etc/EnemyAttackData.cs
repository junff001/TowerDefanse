using UnityEngine;

[System.Serializable]
public class EnemyAttackData
{
    public int blinkingCount;
    public float blinkingDelay;
    public float explosionDamage;
    public float atkRange;
    public float suicideRange = 1.41f;
    public float attackSpeed;
    public LayerMask opponentLayer;
}