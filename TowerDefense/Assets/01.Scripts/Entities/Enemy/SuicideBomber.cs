using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuicideBomber : EnemyBase
{
    [SerializeField] int blinkingCount;
    [SerializeField] float blinkingDelay;
    [SerializeField] float explosionDamage;
    [SerializeField] float atkRangeRadius;
    [SerializeField] LayerMask opponentLayer;

    bool canSuicideBombing = false;
    SpriteRenderer spriteRenderer;
    Transform target;
    List<Collider2D> opponentColliders = new List<Collider2D>();
    ContactFilter2D contactFilter = new ContactFilter2D();
    
    protected override void Awake()
    {
        base.Awake();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    protected override void Update()
    {
        base.Update();

        if (IsRangeInTarget() > 0)
        {
            float originMoveSpeed = enemyData.MoveSpeed;
            enemyData.MoveSpeed = 0;
            StartCoroutine(TargetDiscoverySiren());

            if (canSuicideBombing)
            {
                enemyData.MoveSpeed = originMoveSpeed;
                TargetChase();

                if (IsCollisionTarget())
                {
                    SuicideBombing();
                }
            }
        }
    }

    void SuicideBombing()
    {
        target.GetComponent<HealthSystem>().TakeDamage(enemyData.OffensePower);
        Destroy(this);
    }

    void TargetChase()
    {
        target = opponentColliders[0].transform;
        transform.Translate(target.position * enemyData.MoveSpeed * Time.deltaTime);
    }

    int IsRangeInTarget()
    {
        contactFilter.SetLayerMask(opponentLayer);
        return Physics2D.OverlapCircle(transform.position, atkRangeRadius, contactFilter, opponentColliders);
    }

    bool IsCollisionTarget()
    {
        if (Vector2.Distance(transform.position, target.position) <= 0.1f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    IEnumerator TargetDiscoverySiren()
    {
        int originCount = blinkingCount;

        while (blinkingCount <= 0)
        {
            spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(blinkingDelay);
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(blinkingDelay);
            blinkingCount--;
        }


        blinkingCount = originCount;
        canSuicideBombing = true;
    }
}
