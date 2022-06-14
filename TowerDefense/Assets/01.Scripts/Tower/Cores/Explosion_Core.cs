public class Explosion_Core : CoreBase
{
    public override void Attack(int power, HealthSystem enemy)
    {
        Bomb bullet = Managers.Pool.GetItem<Bomb>();
        bullet.transform.position = transform.position;
        bullet.Init(towerData, enemy.transform);
    }
}
