public class Explosion_Core : CoreBase
{
    public override void Attack(float power, HealthSystem enemy)
    {
        bullet = Managers.Pool.GetItem<Bomb>();
        bullet.transform.position = transform.position;
        bullet.InitProjectileData(power, enemy.transform, Buff);
        OnAttack();
    }
}
