using static Define;

[System.Serializable]
public class EnemyData : LivingEntityData
{
    public int OffensePower;
    public int RewardGold;
    public float MoveSpeed;
    public MonsterType MonsterType;
    public SpeciesType SpeciesType;

    public bool IsHide = false;
    public bool IsShilde = false;
    public bool IsArmor = false;
    public bool IsWitch = false;
    public bool IsAlchemist = false;
    public bool IsFly = false;
    public bool IsThrower = false;

    public void InitEnemyData(EnemySO enemySO, float addPercentEnemyHP)
    {
        MonsterType = enemySO.monsterType;
        SpeciesType = enemySO.speciesType;

        IsHide = MonsterType.HasFlag(MonsterType.Hide);
        IsShilde = MonsterType.HasFlag(MonsterType.Shield);
        IsArmor = MonsterType.HasFlag(MonsterType.Armor);
        IsFly = MonsterType.HasFlag(MonsterType.Fly);
        IsThrower = Managers.Invade.isSpawnningThrower;
        HP += enemySO.HP * addPercentEnemyHP;
        ShieldAmount = IsShilde ? HP : 0; // 쉴드 있는 애면 체력만큼 받아가기
        MoveSpeed = enemySO.moveSpeed;
        RewardGold = enemySO.rewardGold;
    }
}