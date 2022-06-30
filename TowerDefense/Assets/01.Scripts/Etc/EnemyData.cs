using static Define;

[System.Serializable]
public class EnemyData
{
    public int OffensePower;
    public int RewardGold;
    public float HP;
    public float Shield;
    public float MoveSpeed;
    public MonsterType MonsterType;
    public SpeciesType SpeciesType;
    public AttackType AttackType;

    public bool IsHide = false;
    public bool IsShilde = false;
    public bool IsArmor = false;
    public bool IsWitch = false;
    public bool IsAlchemist = false;
    public bool IsFly = false;
    public bool IsSuicideBomber = false;
    public bool IsThrower = false;

    public void InitEnemyData(EnemySO enemySO, float addPercentEnemyHP)
    {
        HP = enemySO.HP;
        Shield = enemySO.ShieldAmount;
        OffensePower = enemySO.OffensePower;
        MoveSpeed = enemySO.MoveSpeed;
        RewardGold = enemySO.RewardGold;
        MonsterType = enemySO.MonsterType;
        SpeciesType = enemySO.SpeciesType;
        IsHide = MonsterType.HasFlag(MonsterType.Hide);
        IsShilde = MonsterType.HasFlag(MonsterType.Shield);
        IsArmor = MonsterType.HasFlag(MonsterType.Armor);
        IsFly = MonsterType.HasFlag(MonsterType.Fly);
        IsSuicideBomber = AttackType.HasFlag(AttackType.SuicideBomber);
        IsThrower = AttackType.HasFlag(AttackType.Thrower);

        HP += HP * addPercentEnemyHP;
    }
}