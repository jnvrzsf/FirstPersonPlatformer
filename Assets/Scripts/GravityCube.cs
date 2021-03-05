using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCube : Pickupable
{
    public override void SetToPickedUp(Carrier c)
    {
        carrier = c;
        rb.useGravity = false;
    }

    public override void SetToDropped()
    {
        carrier = null;
        rb.useGravity = true;
    }
}
