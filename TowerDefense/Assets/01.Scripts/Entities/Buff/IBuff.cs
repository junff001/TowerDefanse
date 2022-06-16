using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Define;

public interface IBuff
{
    public float duration { get; set; }
    public float amplification { get; set; }
    public GameObject affecter { get; set; }
    public BuffType buffType { get; set; }
    public PropertyType propertyType { get; set; }
     
    public void Update();
    public void Destroy();
}
