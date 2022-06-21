using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chain : BuffBase
{
    public Chain(GameObject affecter, float amplification)
    {
        base.amplification = amplification;
        base.affecter = affecter;
    }
}
