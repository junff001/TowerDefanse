using UnityEngine;

public class Explosion_Core : CoreBase
{
    public override void Attack(int power, HealthSystem enemy)
    {
        bullet = Managers.Pool.GetItem<Bomb>();
        bullet.transform.position = transform.position;
        bullet.Init(TowerData, enemy.transform, Buff);
        OnAttack();
    }
}
