using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevitatingCube : Pickupable
{
    public override void SetToPickedUp(Carrying c)
    {
        carrier = c;
        rb.isKinematic = false;
        gameObject.layer = 11;
    }

    public override void SetToDropped()
    {
        carrier = null;
        rb.isKinematic = true;
        gameObject.layer = 10;
    }
}
