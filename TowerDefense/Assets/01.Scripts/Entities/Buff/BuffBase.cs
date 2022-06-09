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
    public BuffBase(GameObject _affecter, float _duration, float _amplification)
    {
        affecter = _affecter;
        duration = _duration;
        amplification = _amplification;
        Init();
    }

    public virtual void Init()
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
