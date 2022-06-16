using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour, IBuff
{
    public float duration { get; set; }
    public float amplification { get; set; }
    public bool isEnd { get; set; }
    public GameObject affecter { get; set; }
    public Define.BuffType buffType { get; set; }
    public Define.PropertyType propertyType { get; set; }

    // 내부 변수
    float radius;
    float damage;
    Collider2D[] enemies;
    ParticleSystem effect;
    LayerMask enemyMask;
    
   
    void OnEnable()
    {
        enemies = InRangeEnemy(enemyMask);

        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].GetComponent<HealthSystem>().TakeDamage(damage, propertyType);
        }

        effect.Play();
    }

    void Update()
    {
        if (effect.isStopped)
        {
            gameObject.SetActive(false);
        }
    }

    public void Initialization(float radius, float damage, ParticleSystem effect, LayerMask enemyMask, Define.PropertyType propertyType)
    {
        this.radius = radius;
        this.damage = damage;
        this.effect = effect;
        this.enemyMask = enemyMask;
        this.propertyType = propertyType;
    }

    Collider2D[] InRangeEnemy(LayerMask mask)
    {
        return Physics2D.OverlapCircleAll(transform.position, radius, mask);
    }

    public void Initialization()
    {
        
    }

    void IBuff.Update()
    {
        
    }

    public void Destroy()
    {
       
    }
}
