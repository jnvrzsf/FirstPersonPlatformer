using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsCube : Pickupable
{
    public override void SetToPickedUp(Carrying c)
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
