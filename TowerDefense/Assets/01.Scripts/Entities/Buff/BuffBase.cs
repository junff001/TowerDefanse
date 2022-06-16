using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BuffBase : IBuff
{
    public float duration { get; set; }
    public float amplification { get; set; }
    public bool isEnd { get; set; } = false;
    public GameObject affecter { get; set; }
    public BuffType buffType { get; set; }
    public PropertyType propertyType { get; set; }

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
