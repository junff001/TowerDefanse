using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockBack : BuffBase
{
    Transform attacker;

    public KnockBack(GameObject affecter, float duration, float amplification, Transform attacker) : base(affecter, duration, amplification)    
    {
        this.attacker = attacker;
        buffType = Define.BuffType.DEBUFF;
    }

    public override void Update()
    {
        base.Update();

        Vector2 direction = affecter.transform.position - attacker.position;
        affecter.transform.Translate(direction.normalized * amplification);
    }
}
