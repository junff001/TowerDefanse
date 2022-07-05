using UnityEngine;


public class Explosion_Core : CoreBase
{
    public override void Attack(HealthSystem enemy, LayerMask opponentLayer)
    {
        bullet = Managers.Pool.GetItem<Bomb>();
        bullet.transform.position = transform.position;
        bullet.InitProjectileData(enemy.transform, Buff, opponentLayer);
        OnAttack();
    }
}
