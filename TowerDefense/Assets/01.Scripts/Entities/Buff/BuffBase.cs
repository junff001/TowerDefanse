using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public class BuffBase
{
    public float duration { get; set; }
    public float amplification { get; set; }
    public bool isEnd = false;
    public GameObject affecter { get; set; }
    public BuffType buffType { get; set; }
    public PropertyType propertyType { get; set; }
  
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
