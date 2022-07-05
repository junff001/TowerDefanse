using System.Collections; 
using System.Collections.Generic;
using UnityEngine;

public class Goblin : Enemy
{
    protected override void Start()
    {
        base.Start();
        healthSystem.OnDamaged += CallHealthSystemOnDamaged;
    }

    private void CallHealthSystemOnDamaged()
    {
        if (!healthSystem.IsDead())
        {
            Managers.Sound.Play("Goblin/GoblinDamage");
        }
    }
}
