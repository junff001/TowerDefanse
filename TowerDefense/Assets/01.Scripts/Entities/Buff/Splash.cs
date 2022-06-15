using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : MonoBehaviour
{
    // 초기화 변수
    float radius;
    float damage;
    float duration;
    ParticleSystem effect;
    LayerMask enemyMask;
    Define.PropertyType propertyType;

    // 내부 변수
    Collider2D[] enemies;

    void OnEnable()
    {
        StartCoroutine(OnSplash(duration));
    }

    void OnDisable()
    {
        StopCoroutine(OnSplash(duration));
    }

    public void Initialization(float radius, float damage, float duration, ParticleSystem effect, LayerMask enemyMask, Define.PropertyType propertyType)
    {
        this.radius = radius;
        this.damage = damage;
        this.duration = duration;
        this.effect = effect;
        this.enemyMask = enemyMask;
        this.propertyType = propertyType;
    }

    IEnumerator OnSplash(float duration)
    {
        while (true)
        {
            for (int i = 0; i < enemies.Length; i++)
            {
                enemies[i].GetComponent<HealthSystem>().TakeDamage(damage, propertyType);
            }

            effect.Play();

            yield return null;  
        }
    }

    Collider2D[] InRangeEnemy()
    {
        enemies = Physics2D.OverlapCircleAll(transform.position, radius, enemyMask);
        return enemies;
    }


}
