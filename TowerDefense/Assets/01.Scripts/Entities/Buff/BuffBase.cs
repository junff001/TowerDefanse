using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffBase 
{
    public float duration;
    public float amplification;

    public bool isEnd;
    public GameObject affecter;

    public Define.BuffType buffType;
    [SerializeField] protected Define.PropertyType propertyType;

    public BuffBase(GameObject affecter, float duration, float amplification)
    {
        this.affecter = affecter;
        this.duration = duration;
        this.amplification = amplification;
        Initialization();
    }

    public virtual void Initialization()
    {
        
    }

    public virtual void Update()
    {
        if (duration < 0)
        {
            Destroy();
        }
        else
        {
            duration -= Time.deltaTime;
        }
    }

    public virtual void Destroy()
    {
        isEnd = true;
    }
}
