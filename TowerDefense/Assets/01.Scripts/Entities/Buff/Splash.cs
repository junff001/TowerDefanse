using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : BuffBase
{
    // 내부 변수
    float radius;
    float damage;
    Collider2D[] enemies;
    ParticleSystem effect;
    LayerMask enemyMask;
      
    public Splash(float radius, float damage, ParticleSystem effect, LayerMask enemyMask, Define.PropertyType propertyType)
    {
        this.radius = radius;
        this.damage = damage;
        this.effect = effect;
        this.enemyMask = enemyMask;
        this.propertyType = propertyType;
    }

    void OnEnable()
    {
        enemies = InRangeEnemy(enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<HealthSystem>().TakeDamage(damage, propertyType);
        }

        effect.Play();
    }

    public override void Update()
    {
        base.Update();

        if (effect.isStopped)
        {
            Destroy();
        }
    }

    Collider2D[] InRangeEnemy(LayerMask mask)
    {
        return Physics2D.OverlapCircleAll(affecter.transform.position, radius, mask);
    }
}
