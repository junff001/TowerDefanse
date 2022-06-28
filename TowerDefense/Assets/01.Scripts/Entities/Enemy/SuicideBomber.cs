using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : EnemyBase
{
    [SerializeField] int countdown;
    [SerializeField] int blinkingCount;
    [SerializeField] float explosionDamage;
    [SerializeField] float blinkingDelay;

    bool canSuicideBombing = false;
    SpriteRenderer spriteRenderer;

    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void SuicideBombing(HealthSystem target)
    {
        target.TakeDamage(explosionDamage);
        Destroy(this);
    }


    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(countdown); 
    }

    IEnumerator TargetDiscoverySiren()
    {
        int originCount = blinkingCount;

        while (blinkingCount <= 0)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkingDelay);
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkingDelay);
            blinkingCount--;
        }

        blinkingCount = originCount;
        canSuicideBombing = true;
    }
}
